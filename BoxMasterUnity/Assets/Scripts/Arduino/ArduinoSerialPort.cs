using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public abstract class ArduinoSerialPort : MonoBehaviour {

    protected SerialPort[] _serialPorts = new SerialPort[GameSettings.PlayerNumber];
    protected Thread _serialThread;
    protected bool _gameRunning = true;
    protected Object serialPortLocker = new Object();

    protected SerialPort OpenSerialPort(int playerIndex, SerialPortSettings serialPortSettings)
    {
        _serialPorts[playerIndex] = new SerialPort(serialPortSettings.name, serialPortSettings.baudRate);
        Debug.Log("Connection started");
        try
        {
            _serialPorts[playerIndex].Open();
            _serialPorts[playerIndex].ReadTimeout = serialPortSettings.readTimeOut;
            _serialPorts[playerIndex].Handshake = (Handshake)serialPortSettings.handshake;

            SendSerialMessage("connect", playerIndex);
            Debug.Log("Port Opened!");
        }
        catch (System.Exception e)
        {
            Debug.Log("Could not open serial port");
            throw e;
        }
        return _serialPorts[playerIndex];
    }

    protected abstract void ThreadUpdate();

    public void SendSerialMessage(string mess_, int playerIndex)
    {
        lock (serialPortLocker)
        {
            if (_serialPorts[playerIndex].IsOpen)
            {
                _serialPorts[playerIndex].Write(mess_ + '_');
            }
        }
    }

    public byte ReadSerialByte(int playerIndex)
    {
        lock (serialPortLocker)
        {
            return (byte)_serialPorts[playerIndex].ReadByte();
        }
    }

    public bool IsSerialOpen(int playerIndex)
    {
        lock (serialPortLocker)
        {
            return _serialPorts[playerIndex] != null && _serialPorts[playerIndex].IsOpen;
        }
    }

    public void CloseSerialPort(int playerIndex)
    {
        lock (serialPortLocker)
        {
            _serialPorts[playerIndex].Close();
        }
    }

    public void OnApplicationQuit()
    {
        _gameRunning = false;
        if (_serialThread != null)
            _serialThread.Abort();

        for (int i = 0; i < _serialPorts.Length; i++)
        {
            if (_serialPorts[i] != null)
            {
                if (_serialPorts[i].IsOpen)
                {
                    SendSerialMessage("disconnect", i);  // stop leds
                    print("closing serial port");
                    CloseSerialPort(i);
                    print("serial port closed");
                }
                _serialPorts[i] = null;
            }
        }
    }
}
