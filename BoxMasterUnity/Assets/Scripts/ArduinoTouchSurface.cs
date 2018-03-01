using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Threading;

public class ArduinoTouchSurface : MonoBehaviour
{
	// Thread variables
	public Thread serialThread;
	public SerialPort serial;

	private Queue<string> _myQueue = new Queue<string> ();
	private int _dataCounter = 0;
	private List<Vector3> _accCollection = new List<Vector3> (); // collection storing acceleration data to compute moving mean

	// Sensor grid setup
	public int ROWS = 8;
	public int COLS = 8;

	// Sensor grid variables
	public GameObject datapointPrefab;
	public GameObject[,] pointGrid;

	// Accelerometer variables
	public Vector3 acceleration = Vector3.zero;
	public int nAcc = 5; // max size of accCollection (size of filter)

	void Start ()
	{
		// Initialize point grid as gameobjects
		pointGrid = new GameObject[ROWS, COLS];
		for (int i = 0; i < ROWS; i++) {
			for (int j = 0; j < COLS; j++) {
				float x_ = i * 10;
				float y_ = j * 10;
				pointGrid [i, COLS - j - 1] = GameObject.Instantiate (datapointPrefab, new Vector3 (x_, y_, 0), Quaternion.identity);
			}
		}

		var serialPortSettings = GameManager.instance.gameSettings.touchSurfaceSerialPort;
		serial = new SerialPort (serialPortSettings.name, serialPortSettings.baudRate);
		Connect ();
		StartCoroutine (PrintSerialDataRate (1f));
	}

	void Connect ()
	{
		Debug.Log ("Connection started");
		try {
			serial.Open ();
			serial.ReadTimeout = GameManager.instance.gameSettings.touchSurfaceSerialPort.readTimeOut;
			serial.Handshake = Handshake.None;
			serialThread = new Thread (RecDataThread);
			serialThread.Start ();
			Debug.Log ("Port Opened!");
		} catch {
			Debug.Log ("Could not open serial port");
		}
	}

	public void OnApplicationQuit ()
	{
		if (serial != null) {
			if (serial.IsOpen) {
				print ("closing serial port");
				serial.Close ();
			}
			serial = null;
		}
	}

	void RecDataThread ()
	{
		if ((serial != null) && (serial.IsOpen)) {
			byte tmp;
			string data = "";
			tmp = (byte)serial.ReadByte ();
			while (tmp != 255) {
				tmp = (byte)serial.ReadByte ();
				if (tmp != 'q') {
					data += ((char)tmp);
				} else {
					_myQueue.Enqueue (data);
					data = "";
				}
			}
		}
	}

	void Update ()
	{
		// Get serial data from second thread
		if (_myQueue != null && _myQueue.Count > 0) {
			int q_length_touch = _myQueue.Count;
			for (int i = 0; i < q_length_touch; i++) {
				string rawdatStr_ = _myQueue.Dequeue ();
				if (rawdatStr_ != null) {
					GetSerialData (rawdatStr_);
				}
			}
		}

		// Remap and display data points
		for (int i = 0; i < ROWS; i++) {
			// Get row data range
			float minRow_ = 1000.0f;
			float maxRow_ = -1000.0f;
			float sumRow_ = 0.0f;
			for (int j = 0; j < COLS; j++) {
				sumRow_ += pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;

				if (minRow_ > pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal) {
					minRow_ = pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;
				}
				if (maxRow_ < pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal) {
					maxRow_ = pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal;
				}
			}

			// Get remap values for the current row and display data point
			for (int j = 0; j < COLS; j++) {
				if (maxRow_ - minRow_ != 0) {
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal = (pointGrid [i, j].GetComponent<DatapointControl> ().curSRelativeVal - minRow_) / (maxRow_ - minRow_);
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal *= sumRow_;
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal /= 1024.0f; // 1024 = max analog range
					pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal = Mathf.Clamp (pointGrid [i, j].GetComponent<DatapointControl> ().curRemapVal, 0.0f, 1.0f);
				}
			}
		}
	}

	void GetSerialData (string serialdata_)
	{		
		serialdata_ = serialdata_.Trim ();

		// First character of the string is an adress
		char adr_ = serialdata_.ToCharArray () [0]; // get address character
		serialdata_ = serialdata_.Split (adr_) [1]; // remove adress from the string to get the message content

		switch (adr_) {
		case 'z':
					// GET COORDINATES
			if (serialdata_ != null) {
				if (serialdata_.Length > 2 + COLS * 2 - 1) {
					int[] rawdat_ = serialdata_.Split ('x').Select (str => int.Parse (str)).ToArray ();
					if (rawdat_.Length == COLS + 1) { // COLS + 1 ROW
						int j = rawdat_ [0];
						for (int k = 1; k < rawdat_.Length; k++) {
							pointGrid [j, k - 1].GetComponent<DatapointControl> ().PushNewRawVal (rawdat_ [k]);
						}
					}
				}
			}
			break;

		case 'a':
					// GET ACCELERATION
			try {
				int[] acc_ = serialdata_.Split ('c').Select (str => int.Parse (str)).ToArray (); // format = ACCXcACCYcACCZ
				if (acc_.Length == 3) {
					this._accCollection.Add (new Vector3 (acc_ [0], acc_ [1], acc_ [2]));
					while (this._accCollection.Count > this.nAcc) {
						this._accCollection.RemoveAt (0);
					}

					// Compute moving mean filter
					Vector3 smoothAcc_ = Vector3.zero;
					foreach (Vector3 curAcc_ in this._accCollection) {
						smoothAcc_ += curAcc_;
					}
					smoothAcc_ /= (float)this._accCollection.Count; 

					this.acceleration = smoothAcc_;
					this.acceleration /= 10000f; // map acceleration TO CHANGE
					this._dataCounter++;
				}
			} catch {
				print ("bad string format");
			}
			break;

		default:
			break;
		}
	}

	IEnumerator PrintSerialDataRate (float waitTime)
	{
		while (true) {
			yield return new WaitForSeconds (waitTime);
			print ("Serial data rate = " + this._dataCounter / waitTime + " data/s");
			this._dataCounter = 0;
		}
	}
}