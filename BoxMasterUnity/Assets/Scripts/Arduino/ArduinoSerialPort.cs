using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public abstract class ArduinoSerialPort : MonoBehaviour
{

    protected SerialPort _serialPort;
    private readonly Object _serialPortLocker = new Object();

    protected Thread _serialThread;
    protected bool _gameRunning = true;
    protected bool _sendMessages = true;

    protected SerialPort OpenSerialPort(SerialPortSettings serialPortSettings)
    {
        _serialPort = new SerialPort(serialPortSettings.name, serialPortSettings.baudRate);
        Debug.Log("Connection started");
        try
        {
            _serialPort.Open();
            _serialPort.ReadTimeout = serialPortSettings.readTimeOut;
            _serialPort.Handshake = (Handshake)serialPortSettings.handshake;

            Debug.Log("Serial Port " + _serialPort.PortName);

            _serialThread = new Thread(() => ThreadUpdate());
            _serialThread.Start();

            if (_sendMessages)
                SendSerialMessage("connect");
            Debug.Log("Port Opened!");
        }
        catch (System.Exception e)
        {
            Debug.Log("Could not open serial port");
            throw e;
        }
        return _serialPort;
    }

    protected abstract void ThreadUpdate();

    public void SendSerialMessage(string mess_)
    {
        lock (_serialPortLocker)
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Write(mess_ + '_');
            }
        }
    }

    public byte ReadSerialByte()
    {
        lock (_serialPortLocker)
        {
            return (byte)_serialPort.ReadByte();
        }
    }

    public bool IsSerialOpen()
    {
        lock (_serialPortLocker)
        {
            return _serialPort != null && _serialPort.IsOpen;
        }
    }

    public void CloseSerialPort()
    {
        lock (_serialPortLocker)
        {
            _serialPort.Close();
        }
    }

    public void OnApplicationQuit()
    {
        _gameRunning = false;
        if (_serialThread != null)
            _serialThread.Abort();

        if (_serialPort != null && _serialPort.IsOpen)
        {
            if (_sendMessages)
                SendSerialMessage("disconnect");  // stop leds
            print("closing serial port");
            CloseSerialPort();
            print("serial port closed");
        }
    }
}
