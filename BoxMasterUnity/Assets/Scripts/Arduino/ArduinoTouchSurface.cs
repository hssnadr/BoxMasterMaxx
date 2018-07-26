// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using UnityEngine.UI;

public class ArduinoTouchSurface : ArduinoSerialPort
{
    // Thread variables
    private Queue<string>[] _dataQueues = new Queue<string>[GameSettings.PlayerNumber];
    private readonly Object[] _dataQueueLockers = new Object[GameSettings.PlayerNumber];
    private int _dataCounter = 0;

    // Sensor grid setup
    [SerializeField]
    private int _rows = 24;
    [SerializeField]
    private int _cols = 24;

    // Sensor grid variables
    [SerializeField]
    protected DatapointControl _datapointPrefab;
    [SerializeField]
    protected ImpactPointControl _impactPointControlPrefab;
    private DatapointControl[,,] _pointGrid;

    // Accelerometer variables
    public Vector3[] acceleration = new Vector3[GameSettings.PlayerNumber];
    private List<Vector3> _accCollection = new List<Vector3>(); // collection storing acceleration data to compute moving mean
    public int nAcc = 5; // max size of accCollection (size of filter)

    protected override void Start()
    {
        // prevent the touch surface to send messages.
        _sendMessages = false;
        base.Start();

        // Initialize point grid as gameobjects
        
        _rows = GameManager.instance.gameSettings.touchSurfaceGrid.rows;
        _cols = GameManager.instance.gameSettings.ledControlGrid.cols;
        _pointGrid = new DatapointControl[GameSettings.PlayerNumber, _rows, _cols];
        int count = 0;
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            Camera camera = GameManager.instance.GetCamera(p).GetComponent<Camera>();
            Debug.Log(camera);
            var grid = new GameObject("Player" + (p + 1) + " Grid");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    float x = i * ((float)camera.orthographicSize / (float)_cols);
                    float y = j * ((float)(camera.orthographicSize * camera.aspect) / (float)_rows);
                    var dpc = GameObject.Instantiate(_datapointPrefab, camera.ScreenToWorldPoint(new Vector3(x, y, 0)), Quaternion.identity, grid.transform);
                    dpc.name = "Datapoint " + count + " (" + x + ";" + y + ")";
                    count++;
                    dpc.playerIndex = p;
                    _pointGrid[p, i, _cols - j - 1] = dpc;
                }
            }
            var ipc = GameObject.Instantiate(_impactPointControlPrefab, this.transform);
            ipc.playerIndex = p;
        }

        for (int i = 0; i < _dataQueues.Length; i++)
            _dataQueues[i] = new Queue<string>();

        for (int i = 0; i < _dataQueueLockers.Length; i++)
            _dataQueueLockers[i] = new Object();

        foreach (string str in SerialPort.GetPortNames())
        {
            Debug.Log(str);
        }
        SerialPortSettings[] serialPortSettings = GameManager.instance.gameSettings.touchSurfaceSerialPorts;
        try
        {
            for (int p = 0; p < GameSettings.PlayerNumber; p++)
            {
                OpenSerialPort(p, serialPortSettings[p]);
            }
            StartCoroutine(PrintSerialDataRate(1f));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    protected override void ThreadUpdate(int playerIndex)
    {
        string data = "";
        byte tmp = 0;
        tmp = (byte)ReadSerialByte(playerIndex);
        while (tmp != 255 && _gameRunning)
        {
            tmp = (byte)_serialPorts[playerIndex].ReadByte();
            if (tmp != 'q')
            {
                data += ((char)tmp);
            }
            else
            {
                Enqueue(data, playerIndex);
                data = "";
            }
        }
    }

    private void Enqueue(string data, int playerIndex)
    {
        lock (_dataQueueLockers[playerIndex])
        {
            Debug.Log("Enqueue");
            _dataQueues[playerIndex].Enqueue(data);
        }
    }

    private string Dequeue(int playerIndex)
    {
        lock (_dataQueueLockers[playerIndex])
        {
            return _dataQueues[playerIndex].Dequeue();
        }
    }

    private int QueueLength(int playerIndex)
    {
        lock (_dataQueueLockers[playerIndex])
        {
            return _dataQueues[playerIndex].Count;
        }
    }

    private void Update()
    {
        // Get serial data from second thread
        for (int playerIndex = 0; playerIndex < GameSettings.PlayerNumber; playerIndex++)
        {
            int count = QueueLength(playerIndex);
            for (int i = 0; i < count; i++)
            {
                string rawDataStr = Dequeue(playerIndex);
                if (rawDataStr != null && rawDataStr.Length > 1)
                {
                    GetSerialData(rawDataStr, playerIndex);
                    GameManager.instance.GetConsoleText((playerIndex + 1)).text = rawDataStr;
                }
            }
        }
        

        // Remap and display data points
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            for (int i = 0; i < _rows; i++)
            {
                // Get row data range
                float minRow_ = 1000.0f;
                float maxRow_ = -1000.0f;
                float sumRow_ = 0.0f;
                for (int j = 0; j < _cols; j++)
                {
                    float curSRelativeVal = _pointGrid[p, i, j].curSRelativeVal;
                    sumRow_ += curSRelativeVal;

                    if (minRow_ > curSRelativeVal)
                    {
                        minRow_ = curSRelativeVal;
                    }
                    if (maxRow_ < curSRelativeVal)
                    {
                        maxRow_ = curSRelativeVal;
                    }
                }

                // Get remap values for the current row and display data point
                for (int j = 0; j < _cols; j++)
                {
                    if (maxRow_ - minRow_ != 0)
                    {
                        float curRemapVal = _pointGrid[p, i, j].curRemapVal;
                        curRemapVal = (_pointGrid[p, i, j].curSRelativeVal - minRow_) / (maxRow_ - minRow_);
                        curRemapVal *= sumRow_;
                        curRemapVal /= 1024.0f; // 1024 = max analog range
                        _pointGrid[p, i, j].curRemapVal = Mathf.Clamp(curRemapVal, 0.0f, 1.0f);
                    }
                }
            }
        }
    }

    private void GetSerialData(string serialData, int p)
    {
        serialData = serialData.Trim();

        // First character of the string is an adress
        char adr_ = serialData.ToCharArray()[0]; // get address character
        serialData = serialData.Split(adr_)[1]; // remove adress from the string to get the message content

        switch (adr_)
        {
            case 'z':
                // GET COORDINATES
                if (serialData != null)
                {
                    if (serialData.Length == 2 + 4 * _cols)
                    {
                        //int[] rawdat_ = serialdata_.Split ('x').Select (str => int.Parse (str)).ToArray (); // get 
                        int[] rawdat_ = serialData.Split('x').Select(str => int.Parse(str, System.Globalization.NumberStyles.HexNumber)).ToArray();
                        //print (rawdat_.Length);
                        //print (COLS+1);
                        if (rawdat_.Length == _cols + 1)
                        { // COLS + 1 ROW
                            int j = rawdat_[0];
                            for (int k = 1; k < rawdat_.Length; k++)
                            {
                                _pointGrid[p, j, k - 1].GetComponent<DatapointControl>().PusNewRawVal(rawdat_[k]);
                            }
                        }
                        _dataCounter++;
                    }
                }
                break;

            case 'a':
                // GET ACCELERATION
                if (serialData != null)
                {
                    if (serialData.Length == 3 * 3 + 2)
                    {
                        int[] acc_ = serialData.Split('c').Select(str => int.Parse(str, System.Globalization.NumberStyles.HexNumber)).ToArray();
                        if (acc_.Length == 3)
                        {
                            _accCollection.Add(new Vector3(acc_[0], acc_[1], acc_[2]));
                            while (_accCollection.Count > this.nAcc)
                            {
                                _accCollection.RemoveAt(0);
                            }

                            // Compute moving mean filter
                            Vector3 smoothAcc_ = Vector3.zero;
                            foreach (Vector3 curAcc_ in _accCollection)
                            {
                                smoothAcc_ += curAcc_;
                            }
                            smoothAcc_ /= (float)_accCollection.Count;

                            this.acceleration[p] = smoothAcc_;
                            this.acceleration[p] /= 10000f; // map acceleration TO CHANGE
                            _dataCounter++;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    private IEnumerator PrintSerialDataRate(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            print("Serial data rate = " + _dataCounter / waitTime + " data/s");
            _dataCounter = 0;
        }
    }
}