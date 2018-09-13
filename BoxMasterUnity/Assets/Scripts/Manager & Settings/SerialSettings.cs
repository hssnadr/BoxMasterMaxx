// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace CRI.HitBox.Settings
{
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
        public int readTimeout;
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
            this.readTimeout = readTimeOut;
        }
    }

    /// <summary>
    /// Settings for a grid of leds or a grid of sensors.
    /// </summary>
    [Serializable]
    public struct SerialGrid
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

        public SerialGrid(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
        }
    }

    [Serializable]
    public struct SerialSettings
    {
        /// <summary>
        /// Settings for the touch surface serial port
        /// </summary>
        [XmlArray("touch_controller_settings")]
        [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
        [SerializeField]
        public SerialPortSettings[] touchControllerSettings;
        /// <summary>
        /// The grid for the touch controller.
        /// </summary>
        [XmlElement("touch_controller_grid")]
        public SerialGrid touchControllerGrid;
        /// <summary>
        /// Settings for the led controller serial port
        /// </summary>
        [XmlArray("led_controller _settings")]
        [XmlArrayItem(typeof(SerialPortSettings), ElementName = "port")]
        [SerializeField]
        public SerialPortSettings[] ledControllerSettings;
        /// <summary>
        /// The grid for the led controller.
        /// </summary>
        [XmlElement("led_controller_grid")]
        public SerialGrid ledControllerGrid;
        /// <summary>
        /// Min value to detect impact
        /// </summary>
        [XmlElement("impact_threshold")]
        public int impactThreshold;
        /// <summary>
        /// Minimum time (in ms) between 2 impacts to be validated (minimum 50ms <=> maximum 50 hits/s)
        /// </summary>
        [XmlElement("delay_off_hit")]
        public int delayOffHit;

        public SerialSettings(SerialPortSettings[] touchControllerSettings,
            SerialPortSettings[] ledControllerSettings,
            SerialGrid touchControllerGrid,
            SerialGrid ledControllerGrid,
            int impactThreshold,
            int delayOffHit)
        {
            this.touchControllerSettings = touchControllerSettings;
            this.ledControllerSettings = ledControllerSettings;
            this.touchControllerGrid = touchControllerGrid;
            this.ledControllerGrid = ledControllerGrid;
            this.impactThreshold = impactThreshold;
            this.delayOffHit = delayOffHit;
        }
    }
}
