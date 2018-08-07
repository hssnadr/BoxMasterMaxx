// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoLedControl : ArduinoSerialPort
{
    /// <summary>
    /// The number of rows for the led control. The data will be automatically read from the Game Settings.
    /// </summary>
    private int _rows = 30;
    /// <summary>
    /// The number of columns for the led control. The data will be automatically read from the Game Settings.
    /// </summary>
    private int _cols = 60;
    /// <summary>
    /// Stores the led data (in RGB).
    /// </summary>
    private Color[] _leds;
    /// <summary>
    /// Stores the led data (in RGB).
    /// </summary>
    private Color[] _newLedColor; // store leds data (Red, Green, Blue)
    /// <summary>
    /// Locker for the leds for thread-safe operations.
    /// </summary>
    private Object _ledsLocker = new Object();

    /// <summary>
    /// Index of the player. The led display will depend on the screen of that player.
    /// </summary>
    [Tooltip("Index of the player. The led display will depend on the screen of that player.")]
    public int playerIndex = 0;

    protected void Start()
    {
        // Initialize leds array to store color values
        _rows = GameManager.instance.gameSettings.ledControlGrid.rows;
        _cols = GameManager.instance.gameSettings.ledControlGrid.cols;
        _leds = new Color[_rows * _cols];
        _newLedColor = new Color[_rows * _cols];
        for (int i = 0; i < _cols; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                _leds[GetLedIndex(i, j)] = Color.red;
                _newLedColor[GetLedIndex(i, j)] = Color.black;
            }
        }

        // Initialize serial connection to leds pannel
        SerialPortSettings[] serialPortSettings = GameManager.instance.gameSettings.ledControlSerialPorts;
        try
        {
            OpenSerialPort(serialPortSettings[playerIndex]);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void Update()
    {
        // Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
        SetPixelColor(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().targetTexture.GetRTPixels());
    }

    /// <summary>
    /// Gets the color of a led at a specific index. Thread-safe.
    /// </summary>
    /// <param name="index">The index of the led.</param>
    /// <returns>The color of the led.</returns>
    private Color GetLedColor(int index)
    {
        lock (_ledsLocker)
        {
            return _newLedColor[index];
        }
    }
    
    /// <summary>
    /// Sets the color of a led at a specific index. Thread-safe.
    /// </summary>
    /// <param name="index">The index of the led.</param>
    /// <param name="value">The new color of the led.</param>
    private void SetLedColor(int index, Color value)
    {
        lock (_ledsLocker)
        {
            _newLedColor[index] = value;
        }
    }

    /// <summary>
    /// For each pixel in a given texture, call the SetTheLedColor function to set the color of each led.
    /// </summary>
    /// <param name="screenTexture">The texture that will be given to all the leds.</param>
    private void SetPixelColor(Texture2D screenTexture)
    {
        // get pixel color to drive leds pannel
        int offsetX = (int)(screenTexture.width / 2f - screenTexture.height / 2f);
        for (int i = 0; i < _cols; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                int gx = (int)(screenTexture.height * (i / ((float)_cols))) + offsetX;
                int gy = (int)(screenTexture.height * ((j / ((float)_rows)) * 0.95f + 0.025f));
                SetLedColor(GetLedIndex(i, j), screenTexture.GetPixel(gx, gy));
            }
        }

        Object.Destroy(screenTexture);
    }

    protected override void ThreadUpdate()
    {
        int length = _newLedColor.Length;
        while (_gameRunning)
        {
            for (int i = 0; i < length; i++)
            {
                ThreadWriteLedColor(i, GetLedColor(i));
            }
        }
    }

    /// <summary>
    /// Gets the index of the led with a given x and y.
    /// </summary>
    /// <param name="x">The x coordinate of the led.</param>
    /// <param name="y">The y coordinate of the led.</param>
    /// <returns></returns>
    private int GetLedIndex(int x, int y)
    {
        // Convert X and Y coordinate into the led index
        x = Mathf.Clamp(x, 0, _cols - 1);
        y = Mathf.Clamp(y, 0, _rows - 1);
        if (y % 2 == 0)
        {
            x = _cols - 1 - x;
        }
        y = _rows - 1 - y;
        int ipix = y * _cols + x;
        return ipix;
    }

    /// <summary>
    /// Function called by the thread. Call the write function to send the color of the led at a given index.
    /// </summary>
    /// <param name="ipix">Index of the led.</param>
    /// <param name="col">Color that will be written.</param>
    private void ThreadWriteLedColor(int ipix, Color col)
    {
        string ledSerialData = "";
        // Do not send color value to the led if it_s already the same
        if (_leds[ipix] != col)
        {
            int r = (int)(255 * col.r);
            int g = (int)(255 * col.g);
            int b = (int)(255 * col.b);

            // Create data string to send and send it to serial port
            //ledSerialData = "" + ipix_.ToString("0000") + r_.ToString("000") + g_.ToString("000") + b_.ToString("000") + '_';   // decimal format


            //-----------------------------------------------------
            // TO OPTIMIZE = color code 
            if (r == 0 && g == 0 && b == 0)
                ledSerialData = "" + ipix.ToString("X3") + "0_";
            else if (r == 255 && g == 255 && b == 255)
                ledSerialData = "" + ipix.ToString("X3") + "1_";
            else
                ledSerialData = "" + ipix.ToString("X3") + r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + '_';
            //-----------------------------------------------------

            // Send new color to leds pannel
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(ledSerialData);
                _serialPort.Write(ledSerialData);
                _leds[ipix] = col; // update led color values
            }
        }
    }
}