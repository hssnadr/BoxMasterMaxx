using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public abstract class ArduinoSerialPort : MonoBehaviour {

    protected SerialPort[] _serialPorts = new SerialPort[2];
    protected Thread _serialThread;
    protected bool _gameRunning = false;

    protected SerialPort OpenSerialPort(uint index, SerialPortSettings serialPortSettings)
    {
        var serial = new SerialPort(serialPortSettings.name, serialPortSettings.baudRate);
        Debug.Log("Connection started");
        try
        {
            serial.Open();
            serial.ReadTimeout = serialPortSettings.readTimeOut;
            serial.Handshake = (Handshake)serialPortSettings.handshake;

            SendSerialMessage("Connect");

            _serialThread = new Thread(ThreadUpdate);
            _serialThread.Start();
            Debug.Log("Port Opened!");
        }
        catch
        {
            Debug.Log("Could not open serial port");
        }
        return serial;
    }

    protected abstract void ThreadUpdate();

    public void SendSerialMessage(string mess_, uint index = 0)
    {
        if (_serialPorts[index].IsOpen)
        {
            _serialPorts[index].Write(mess_ + '_');
        }
    }

    public void OnApplicationQuit()
    {
        for (int i = 0; i < _serialPorts.Length; i++)
        {
            if (_serialPorts[i] != null)
            {
                if (_serialPorts[i].IsOpen)
                {
                    SendSerialMessage("disconnect");  // stop leds
                    print("closing serial port");
                    _serialPorts[i].Close();
                }
                _serialPorts[i] = null;
            }
        }

        _gameRunning = false; // strop thread
    }
}
