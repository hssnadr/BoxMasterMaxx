using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

/// <summary>
/// This class represent a connection with Arduino through serial.
/// </summary>
public abstract class ArduinoSerialPort : MonoBehaviour
{
    /// <summary>
    /// The serial port of the arduino connection.
    /// </summary>
    protected SerialPort _serialPort;
    /// <summary>
    /// A locker for the serial port lockers. Prevent the read and write operations to be done at the same time.
    /// </summary>
    private readonly Object _serialPortLocker = new Object();
    /// <summary>
    /// The serial thread that will run independently.
    /// </summary>
    protected Thread _serialThread;
    /// <summary>
    /// If true, the game is running. If false, the thread stops.
    /// </summary>
    protected bool _gameRunning = true;
    /// <summary>
    /// If true, this instance will automatically send messages at the start and the end of the connection.
    /// </summary>
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

    /// <summary>
    /// Sends a serial message. Thread-safe.
    /// </summary>
    /// <param name="mess_">The message that will be sent.</param>
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
    
    /// <summary>
    /// Read a byte from the serial port. Thread-safe.
    /// </summary>
    /// <returns>The byte read.</returns>
    public byte ReadSerialByte()
    {
        lock (_serialPortLocker)
        {
            return (byte)_serialPort.ReadByte();
        }
    }

    /// <summary>
    /// Checks if the serial port is open. Thread-safe.
    /// </summary>
    /// <returns>True if the serial port is open.</returns>
    public bool IsSerialOpen()
    {
        lock (_serialPortLocker)
        {
            return _serialPort != null && _serialPort.IsOpen;
        }
    }

    /// <summary>
    /// Closes the serial port. Thread-safe.
    /// </summary>
    public void CloseSerialPort()
    {
        lock (_serialPortLocker)
        {
            _serialPort.Close();
        }
    }

    private void OnApplicationQuit()
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
