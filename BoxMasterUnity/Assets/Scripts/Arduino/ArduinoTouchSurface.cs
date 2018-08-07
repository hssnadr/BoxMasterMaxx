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
    // Thread attributes.

    /// <summary>
    /// Queue of temporarily stored data. Is used by the thread.
    /// </summary>
    private Queue<string> _dataQueue = new Queue<string>();
    /// <summary>
    /// Locker for all the data queue operations.
    /// </summary>
    private readonly Object _dataQueueLocker = new Object();
    /// <summary>
    /// Counts the number of data read each second.
    /// </summary>
    private int _dataCounter = 0;

    /// <summary>
    /// The player index of the touch surface. All the data read will affect that player's gameplay.
    /// </summary>
    [Tooltip("The player index of the touch surface. All the data read will affect that player's gameplay.")]
    public int playerIndex = 0;

    // Sensor grid setup.
    /// <summary>
    /// The number of rows of the sensor grid. Will be automatically replaced by the Game Settings.
    /// </summary>
    private int _rows = 24;
    /// <summary>
    /// The number of columns of the sensor grid. Will be automatically replaced by the Game Settings.
    /// </summary>
    private int _cols = 24;

    // Sensor grid variables
    /// <summary>
    /// The prefab of the datapoint control. The datapoint will be cloned for each sensor in the grid.
    /// </summary>
    [SerializeField]
    [Tooltip("The prefab of the datapoint control. The datapoint will be cloned for each sensor in the grid.")]
    protected DatapointControl _datapointPrefab;
    /// <summary>
    /// The prefab of the impact point. The impact point manages all the datapoints.
    /// </summary>
    [SerializeField]
    [Tooltip("The prefab of the impact point. The impact point manages all the datapoints.")]
    protected ImpactPointControl _impactPointControlPrefab;
    /// <summary>
    /// A grid of datapoints.
    /// </summary>
    private DatapointControl[,] _pointGrid;
    
    /// <summary>
    /// Acceleration values.
    /// </summary>
    public Vector3 acceleration;
    /// <summary>
    /// Stores acceleration data to compe moving mean.
    /// </summary>
    private List<Vector3> _accCollection = new List<Vector3>();
    /// <summary>
    /// Max sac of accCollection.
    /// </summary>
    const int nAcc = 5; // max size of accCollection (size of filter)

    protected void Start()
    {
        // prevent the touch surface to send messages.
        _sendMessages = false;

        // Initialize point grid as gameobjects

        _rows = GameManager.instance.gameSettings.touchSurfaceGrid.rows;
        _cols = GameManager.instance.gameSettings.touchSurfaceGrid.cols;
        _pointGrid = new DatapointControl[_rows, _cols];
        int count = 0;
        Camera camera = GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>();
        Debug.Log(camera);
        var grid = new GameObject("Player" + (playerIndex + 1) + " Grid");
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
            {
                float x = i * ((float)1.0f / _cols);
                float y = j * ((float)1.0f / _rows);
                var dpc = GameObject.Instantiate(_datapointPrefab, camera.ViewportToWorldPoint(new Vector3(x, y, camera.nearClipPlane)), Quaternion.identity, grid.transform);
                dpc.name = "Datapoint " + count + " " + playerIndex + " (" + x + ";" + y + ")";
                dpc.gameObject.layer = 8 + playerIndex;
                count++;
                dpc.playerIndex = playerIndex;
                _pointGrid[i, _cols - j - 1] = dpc;
            }
        }
        var ipc = GameObject.Instantiate(_impactPointControlPrefab, this.transform);
        ipc.playerIndex = playerIndex;
        SerialPortSettings[] serialPortSettings = GameManager.instance.gameSettings.touchSurfaceSerialPorts;
        try
        {
            OpenSerialPort(serialPortSettings[playerIndex]);
            StartCoroutine(PrintSerialDataRate(1f));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    protected override void ThreadUpdate()
    {
        string data = "";
        byte tmp = 0;
        tmp = ReadSerialByte();
        while (tmp != 255 && _gameRunning)
        {
            tmp = ReadSerialByte();
            if (tmp != 'q')
            {
                data += ((char)tmp);
            }
            else
            {
                Enqueue(data);
                data = "";
            }
        }
    }

    /// <summary>
    /// Enqueue a data in the data queue. Thread-safe.
    /// </summary>
    /// <param name="data">The data that will be enqueued.</param>
    private void Enqueue(string data)
    {
        lock (_dataQueueLocker)
        {
            _dataQueue.Enqueue(data);
        }
    }

    /// <summary>
    /// Dequeue a data at the top of the data queue. Thread-safe.
    /// </summary>
    /// <returns>The data dequeued.</returns>
    private string Dequeue()
    {
        lock (_dataQueueLocker)
        {
            return _dataQueue.Dequeue();
        }
    }

    /// <summary>
    /// The length of the data queue.
    /// </summary>
    /// <returns>The length of the data queue.</returns>
    private int QueueLength()
    {
        lock (_dataQueueLocker)
        {
            return _dataQueue.Count;
        }
    }

    private void Update()
    {
        // Get serial data from second thread
        int count = QueueLength();
        for (int i = 0; i < count; i++)
        {
            string rawDataStr = Dequeue();
            if (rawDataStr != null && rawDataStr.Length > 1)
            {
                ParseSerialData(rawDataStr);
                GameManager.instance.GetConsoleText(playerIndex).text = rawDataStr;
            }
        }


        // Remap and display data points
        for (int i = 0; i < _rows; i++)
        {
            // Get row data range
            float minRow = 1000.0f;
            float maxRow = -1000.0f;
            float sumRow = 0.0f;
            for (int j = 0; j < _cols; j++)
            {
                float curSRelativeVal = _pointGrid[i, j].curSRelativeVal;
                sumRow += curSRelativeVal;

                if (minRow > curSRelativeVal)
                    minRow = curSRelativeVal;
                if (maxRow < curSRelativeVal)
                    maxRow = curSRelativeVal;
            }

            // Get remap values for the current row and display data point
            for (int j = 0; j < _cols; j++)
            {
                if (maxRow - minRow != 0)
                {
                    float curRemapVal = _pointGrid[i, j].curRemapVal;
                    curRemapVal = (_pointGrid[i, j].curSRelativeVal - minRow) / (maxRow - minRow);
                    curRemapVal *= sumRow;
                    curRemapVal /= 1024.0f; // 1024 = max analog range
                    _pointGrid[i, j].curRemapVal = Mathf.Clamp(curRemapVal, 0.0f, 1.0f);
                }
            }
        }
    }

    /// <summary>
    /// Parse the serial data.
    /// </summary>
    /// <param name="serialData">The serial data that will be parsed.</param>
    private void ParseSerialData(string serialData)
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
                                _pointGrid[j, k - 1].GetComponent<DatapointControl>().PusNewRawVal(rawdat_[k]);
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
                            while (_accCollection.Count > nAcc)
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

                            this.acceleration = smoothAcc_;
                            this.acceleration /= 10000f; // map acceleration TO CHANGE
                            _dataCounter++;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _cols; j++)
                if (_pointGrid[i, j] != null)
                {
                    Destroy(_pointGrid[i, j].gameObject);
                    _pointGrid[i, j] = null;
                }
        }
    }

    /// <summary>
    /// Coroutine called each <paramref name="waitTime"/> to print the serial data rate.
    /// </summary>
    /// <param name="waitTime">The wait time between each print.</param>
    /// <returns></returns>
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