using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoLedControl : MonoBehaviour
{
	public SerialPort serial;
	// arduino serial port

	// Led pannel
	const int ROWS = 7;
	const int COLS = 20;


	Color[] _leds; // store leds data (Red, Green, Blue)
	Texture2D _screenTexture;

	// Led variables
	public int iPix = 0;
	public int red = 0;
	public int green = 0;
	public int blue = 0;

	void Start ()
	{
		// Initialize serial connection to leds pannel
		var serialPortSettings = GameManager.instance.gameSettings.ledControlSerialPort;
		serial = new SerialPort (serialPortSettings.name, serialPortSettings.baudRate);
		Debug.Log ("Connection started");
		try {
			serial.Open ();
			serial.ReadTimeout = serialPortSettings.readTimeOut;
			serial.Handshake = Handshake.None;
			Debug.Log ("Port Opened!");
		} catch {
			Debug.Log ("Could not open serial port");
		}

		// Initialize leds array to store color values
		_leds = new Color[ROWS * COLS];
		for (int i = 0; i < COLS; i++) {
			for (int j = 0; j < ROWS; j++) {
				SetLedColor (i,j,Color.black);
			}
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

	void Update ()
	{
		// Get pixel color from camera and send it to the corresponding led on the leds strip
		_screenTexture = ScreenCapture.CaptureScreenshotAsTexture (); 
		for (int i = 0; i < COLS; i ++) {
			for (int j = 0; j < ROWS; j ++) {
				int gx = (int)(_screenTexture.width * (0.8f * i / ((float)COLS) + 0.1f));
				int gy = (int)(_screenTexture.height * (0.8f * j / ((float)ROWS) + 0.1f));
				SetLedColor (i,j,_screenTexture.GetPixel (gx ,gy));
			}
		}
	}

	int GetLedIndex (int x, int y)
	{
		// Convert X and Y coordinate into the led index
		x = Mathf.Clamp (x, 0, COLS - 1);
		y = Mathf.Clamp (y, 0, ROWS - 1);
		if (y % 2 != 0) {
			x = COLS-1 - x;
		}
		y = ROWS-1 - y;
		int ipix = y * COLS + x;
		return ipix;
	}

	public void SetLedColor (int x, int y, Color col)
	{
		int ipix_ = GetLedIndex (x, y); // get led index on the strip

		// Do not send color value to the led if it's already the same
		if (_leds [ipix_] != col) {
			int r_ = (int)(255 * col.r);
			int g_ = (int)(255 * col.g);
			int b_ = (int)(255 * col.b);

			// Create data string to send and send it to serial port
			//string data = "" + System.Convert.ToChar (ipix_) + System.Convert.ToChar (r_) + System.Convert.ToChar (g_) + System.Convert.ToChar (b_) + '_';
			string data = "" + ipix_.ToString("000") + r_.ToString("000") + g_.ToString("000") + b_.ToString("000") + '_';
			//print (data);

			if (serial.IsOpen) {
				serial.Write (data);
				_leds [ipix_] = col; // update led color values
			}
		}
	}
}