// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoLedControl : ArduinoSerialPort
{
    public bool gameRunning = true; // TO CHANGE

    // Led pannel
    private int _rows = 30;
    private int _cols = 60;
    private string _ledSerialData = "";
    private Color[,] _leds; // store leds data (Red, Green, Blue)
    private Color[,] _newLedColor; // store leds data (Red, Green, Blue)

    private void Start()
    {
        // Initialize leds array to store color values
        _rows = GameManager.instance.gameSettings.ledControlGrid.rows;
        _cols = GameManager.instance.gameSettings.ledControlGrid.cols;
        _leds = new Color[GameSettings.PlayerNumber, _rows * _cols];
        _newLedColor = new Color[GameSettings.PlayerNumber, _rows * _cols];
        for (int p = 0; p < 2; p++)
        {
            for (int i = 0; i < _cols; i++)
            {
                for (int j = 0; j < _rows; j++)
                {
                    _leds[p, GetLedIndex(i, j)] = Color.red;
                    _newLedColor[p, GetLedIndex(i, j)] = Color.black;
                }
            }
        }

        // -------------------------------------
        // -------------------------------------
        // Print available serial
        foreach (string str in SerialPort.GetPortNames())
        {
            Debug.Log(str);
        }

        // Initialize serial connection to leds pannel
        SerialPortSettings[] serialPortSettings = GameManager.instance.gameSettings.ledControlSerialPorts;
        _serialPorts[0] = OpenSerialPort(0, serialPortSettings[0]);
        _serialPorts[1] = OpenSerialPort(1, serialPortSettings[1]);
    }

    private void Update()
    { 
        // Texture2D screenTexture = ScreenCapture.CaptureScreenshotAsTexture();
        for (int p = 0; p < GameSettings.PlayerNumber; p++)
        {
            SetPixelColor(GameManager.instance.GetCamera((uint)p).GetComponent<Camera>().targetTexture.GetRTPixels(), p);
        }
    }

    private void SetPixelColor(Texture2D screenTexture, int p)
    {
        // get pixel color to drive leds pannel
        int offsetX = (int)(screenTexture.width / 2f - screenTexture.height / 2f);
        for (int i = 0; i < _cols; i++)
        {
            for (int j = 0; j < _rows; j++)
            {
                int gx = (int)(screenTexture.height * (i / ((float)_cols))) + offsetX;
                int gy = (int)(screenTexture.height * ((j / ((float)_rows)) * 0.95f + 0.025f));
                _newLedColor[p, GetLedIndex(i, j)] = screenTexture.GetPixel(gx, gy);
            }
        }
    }

    protected override void ThreadUpdate()
    {
        while (gameRunning)
        {
            for (int p = 0; p < 2; p++)
            {
                for (int i = 0; i < _newLedColor.Length; i++)
                {
                    SetLedColor(i, _newLedColor[p, i]);
                }
            }
        }
    }

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

    public void SetLedColor(int ipix, Color col, uint p = 0)
    {
        // Do not send color value to the led if it_s already the same
        if (_leds[p, ipix] != col)
        {
            int r = (int)(255 * col.r);
            int g = (int)(255 * col.g);
            int b = (int)(255 * col.b);

            // Create data string to send and send it to serial port
            //ledSerialData = "" + ipix_.ToString("0000") + r_.ToString("000") + g_.ToString("000") + b_.ToString("000") + '_';   // decimal format
            _ledSerialData = "" + ipix.ToString("X3") + r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + '_';

            //-----------------------------------------------------
            // TO OPTIMIZE = color code 
            if (r == 0 && g == 0 && b == 0)
            {
                _ledSerialData = "" + ipix.ToString("X3") + "0_";
            }
            if (r == 255 && g == 255 && b == 255)
            {
                _ledSerialData = "" + ipix.ToString("X3") + "1_";
            }
            //-----------------------------------------------------

            // Send new color to leds pannel
            if (_serialPorts[p].IsOpen)
            {
                //print (ledSerialData);
                _serialPorts[p].Write(_ledSerialData);
                _leds[p, ipix] = col; // update led color values
            }
        }
    }
}