using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// Settings for one of the serial ports.
/// </summary>
[Serializable]
public struct SerialPortSettings
{
    public enum SerializableHandshake
    {
        [XmlEnum(Name = "None")]
        None = 0,
        [XmlEnum(Name = "XOnXOff")]
        XOnXOff = 1,
        [XmlEnum(Name = "RequestToSend")]
        RequestToSend = 2,
        [XmlEnum(Name = "RequestToSendXOnXOff")]
        RequestToSendXOnXOff = 3
    }
    /// <summary>
    /// Name of the serial port
    /// </summary>
    [XmlElement("name")]
    public string name;
    /// <summary>
    /// The baud rate of the serial port
    /// </summary>
    [XmlElement("baud_rate")]
    public int baudRate;
    /// <summary>
    /// The timeout of the read.
    /// </summary>
    [XmlElement("read_timeout")]
    public int readTimeOut;
    /// <summary>
    /// The handshake.
    /// </summary>
    [XmlElement("handshake")]
    public SerializableHandshake handshake;

    public SerialPortSettings(string name, int baudRate, SerializableHandshake handshake, int readTimeOut = 0)
    {
        this.name = name;
        this.baudRate = baudRate;
        this.handshake = handshake;
        this.readTimeOut = readTimeOut;
    }
}

/// <summary>
/// Settings for a grid of leds or a grid of sensors.
/// </summary>
[Serializable]
public struct ArduinoGrid
{
    /// <summary>
    /// The number of rows.
    /// </summary>
    [XmlAttribute("rows")]
    public int rows;
    /// <summary>
    /// The number of columns.
    /// </summary>
    [XmlAttribute("cols")]
    public int cols;

    public ArduinoGrid(int rows, int cols)
    {
        this.rows = rows;
        this.cols = cols;
    }
}

[Serializable]
public struct ArduinoSettings
{
    /// <summary>
    /// Settings for the touch surface serial port
    /// </summary>
    [XmlArray("touch_surface_serial_ports")]
    [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
    [SerializeField]
    public SerialPortSettings[] touchSurfaceSerialPorts;
    /// <summary>
    /// The grid for the touch surface.
    /// </summary>
    [XmlElement("touch_surface_grid")]
    public ArduinoGrid touchSurfaceGrid;
    /// <summary>
    /// Settings for the led control serial port
    /// </summary>
    [XmlArray("led_control_serial_ports")]
    [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
    [SerializeField]
    public SerialPortSettings[] ledControlSerialPorts;
    /// <summary>
    /// The grid for the led control.
    /// </summary>
    [XmlElement("led_control_grid")]
    public ArduinoGrid ledControlGrid;
    /// <summary>
    /// Min value to detect impact
    /// </summary>
    [XmlElement("impact_threshold")]
    public int impactThreshold;
    /// <summary>
    /// The buttons of the menu in the order they appear from left to right. The start button will be in all the enabled langages.
    /// </summary>

    public ArduinoSettings(SerialPortSettings[] touchSurfaceSerialPorts,
        SerialPortSettings[] ledControlSerialPorts,
        ArduinoGrid touchSurfaceGrid,
        ArduinoGrid ledControlGrid,
        int impactThreshold)
    {
        this.touchSurfaceSerialPorts = touchSurfaceSerialPorts;
        this.ledControlSerialPorts = ledControlSerialPorts;
        this.touchSurfaceGrid = touchSurfaceGrid;
        this.ledControlGrid = ledControlGrid;
        this.impactThreshold = impactThreshold;
    }
}
