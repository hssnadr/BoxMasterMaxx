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
    private int _dataCounter = 0;

    // Sensor grid setup
    private int _rows = 24;
    private int _cols = 25;

    // Sensor grid variables
    public DatapointControl datapointPrefab;
    public ImpactPointControl impactPointControlPrefab;
    public DatapointControl[,,] pointGrid;

    // Accelerometer variables
    public Vector3[] acceleration = new Vector3[GameSettings.PlayerNumber];
    private List<Vector3> _accCollection = new List<Vector3>(); // collection storing acceleration data to compute moving mean
    public int nAcc = 5; // max size of accCollection (size of filter)

    private void Start()
    {
        // Initialize point grid as gameobjects

        _rows = GameManager.instance.gameSettings.touchSurfaceGrid.rows;
        _cols = GameManager.instance.gameSettings.ledControlGrid.cols;
        pointGrid = new DatapointControl[GameSettings.PlayerNumber, _rows, _cols];
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            Bounds bounds = GameManager.instance.GetCamera(p).bounds;
            var grid = new GameObject("Player" + (p + 1) + " Grid");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    float x_ = bounds.min.x + i * ((bounds.extents.x * 2) / _cols);
                    float y_ = bounds.min.y + j * ((bounds.extents.y * 2) / _rows);
                    var dpc = GameObject.Instantiate(datapointPrefab, new Vector3(x_, y_, 0), Quaternion.identity, grid.transform);
                    dpc.playerIndex = p;
                    pointGrid[p, i, _cols - j - 1] = dpc;
                }
            }
            var ipc = GameObject.Instantiate(impactPointControlPrefab, this.transform);
            ipc.playerIndex = p;
        }

        for (int i = 0; i < _dataQueues.Length; i++)
            _dataQueues[i] = new Queue<string>();

        foreach (string str in SerialPort.GetPortNames())
        {
            Debug.Log(str);
        }
        SerialPortSettings[] serialPortSettings = GameManager.instance.gameSettings.touchSurfaceSerialPorts;
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            _serialPorts[p] = OpenSerialPort(p, serialPortSettings[p]);
        }
        StartCoroutine(printSerialDataRate(1f));
    }

    protected override void ThreadUpdate()
    {
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            SerialPort serial = _serialPorts[p];
            if ((serial != null) && (serial.IsOpen))
            {
                byte tmp;
                string data = "";
                tmp = (byte)serial.ReadByte();
                while (tmp != 255)
                {
                    tmp = (byte)serial.ReadByte();
                    if (tmp != 'q')
                    {
                        data += ((char)tmp);
                    }
                    else
                    {
                        _dataQueues[p].Enqueue(data);
                        data = "";
                    }
                }
            }
        }
    }

    private void Update()
    {
        // Get serial data from second thread
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            Queue<string> dataQueue = _dataQueues[p];
            if (dataQueue != null && dataQueue.Count > 0)
            {
                int qLengthTouch = dataQueue.Count;
                for (int i = 0; i < qLengthTouch; i++)
                {
                    string rawDataStr = dataQueue.Dequeue();
                    if (rawDataStr != null && rawDataStr.Length > 1)
                    {
                        GetSerialData(rawDataStr, p);
                        GameManager.instance.GetConsoleText((p + 1)).text = rawDataStr;
                    }
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
                    sumRow_ += pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal;

                    if (minRow_ > pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal)
                    {
                        minRow_ = pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal;
                    }
                    if (maxRow_ < pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal)
                    {
                        maxRow_ = pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal;
                    }
                }

                // Get remap values for the current row and display data point
                for (int j = 0; j < _cols; j++)
                {
                    if (maxRow_ - minRow_ != 0)
                    {
                        pointGrid[p, i, j].GetComponent<DatapointControl>().curRemapVal = (pointGrid[p, i, j].GetComponent<DatapointControl>().curSRelativeVal - minRow_) / (maxRow_ - minRow_);
                        pointGrid[p, i, j].GetComponent<DatapointControl>().curRemapVal *= sumRow_;
                        pointGrid[p, i, j].GetComponent<DatapointControl>().curRemapVal /= 1024.0f; // 1024 = max analog range
                        pointGrid[p, i, j].GetComponent<DatapointControl>().curRemapVal = Mathf.Clamp(pointGrid[p, i, j].GetComponent<DatapointControl>().curRemapVal, 0.0f, 1.0f);
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
                                pointGrid[p, j, k - 1].GetComponent<DatapointControl>().PusNewRawVal(rawdat_[k]);
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

    private IEnumerator printSerialDataRate(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            print("Serial data rate = " + _dataCounter / waitTime + " data/s");
            _dataCounter = 0;
        }
    }
}