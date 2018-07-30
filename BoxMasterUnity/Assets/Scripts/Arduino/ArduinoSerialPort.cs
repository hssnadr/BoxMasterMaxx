using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public abstract class ArduinoSerialPort : MonoBehaviour {

    protected SerialPort[] _serialPorts = new SerialPort[GameSettings.PlayerNumber];
    private readonly Object[] _serialPortLockers = new Object[GameSettings.PlayerNumber];

    protected Thread[] _serialThread = new Thread[GameSettings.PlayerNumber];
    protected bool _gameRunning = true;
    protected bool _sendMessages = true;

    protected virtual void Start()
    {
        for (int i = 0; i < _serialPortLockers.Length; i++)
        {
            _serialPortLockers[i] = new Object();
        }
    }

    protected SerialPort OpenSerialPort(int playerIndex, SerialPortSettings serialPortSettings)
    {
        SerialPort serialPort = new SerialPort(serialPortSettings.name, serialPortSettings.baudRate);
        Debug.Log("Connection started");
        try
        {
            serialPort.Open();
            serialPort.ReadTimeout = serialPortSettings.readTimeOut;
            serialPort.Handshake = (Handshake)serialPortSettings.handshake;

            Debug.Log("Serial Port " + serialPort.PortName);

            _serialPorts[playerIndex] = serialPort;

            _serialThread[playerIndex] = new Thread(() => ThreadUpdate(playerIndex));
            _serialThread[playerIndex].Start();

            if (_sendMessages)
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

    protected abstract void ThreadUpdate(int playerIndex);

    public void SendSerialMessage(string mess_, int playerIndex)
    {
        lock (_serialPortLockers[playerIndex])
        {
            if (_serialPorts[playerIndex].IsOpen)
            {
                _serialPorts[playerIndex].Write(mess_ + '_');
            }
        }
    }

    public byte ReadSerialByte(int playerIndex)
    {
        lock (_serialPortLockers[playerIndex])
        {
            return (byte)_serialPorts[playerIndex].ReadByte();
        }
    }

    public bool IsSerialOpen(int playerIndex)
    {
        lock (_serialPortLockers[playerIndex])
        {
            return _serialPorts[playerIndex] != null && _serialPorts[playerIndex].IsOpen;
        }
    }

    public void CloseSerialPort(int playerIndex)
    {
        lock (_serialPortLockers[playerIndex])
        {
            _serialPorts[playerIndex].Close();
        }
    }

    public void OnApplicationQuit()
    {
        _gameRunning = false;
        for (int i = 0; i < _serialThread.Length; i++)
        {
            if (_serialThread == null)
                _serialThread[i].Abort();
        }

        for (int i = 0; i < _serialPorts.Length; i++)
        {
            if (_serialPorts[i] != null)
            {
                if (_serialPorts[i].IsOpen)
                {
                    if (_sendMessages)
                        SendSerialMessage("disconnect", i);  // stop leds
                    print("closing serial port");
                    CloseSerialPort(i);
                    print("serial port closed");
                }
            }
        }
    }
}
