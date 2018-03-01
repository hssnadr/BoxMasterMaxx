using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

[System.Serializable]
public struct SerialPortSettings
{
	/// <summary>
	/// Name of the serial port
	/// </summary>
	public string name;
	/// <summary>
	/// The baud rate of the serial port
	/// </summary>
	public int baudRate;
	/// <summary>
	/// The timeout of the read.
	/// </summary>
	public int readTimeOut;
}

[System.Serializable]
public class GameSettings {
	/// <summary>
	/// Settings for the touch surface serial port
	/// </summary>
	public SerialPortSettings touchSurfaceSerialPort;
	/// <summary>
	/// Settings for the led control serial port
	/// </summary>
	public SerialPortSettings ledControlSerialPort;
	/// <summary>
	/// Min value to detect impact
	/// </summary>
	public int impactThreshold;
	/// <summary>
	/// Time until the application returns to the home screen.
	/// </summary>
	public float timeOut;

	/// <summary>
	/// Save the data to an XML file.
	/// </summary>
	/// <param name="path">The path where the XML file will be saved.</param>
	public void Save(string path)
	{
		var serializer = new XmlSerializer (typeof(GameSettings));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Create)) 
		using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
		{
			xmlWriter.Formatting = Formatting.Indented;
			serializer.Serialize (xmlWriter, this);
		}
	}

	/// <summary>
	/// Loads a game setting data from the XML file at the specified path.
	/// </summary>
	/// <returns>The game setting data.</returns>
	/// <param name="path">The path here the XML file is located.</param>
	public static GameSettings Load(string path)
	{
		var serializer = new XmlSerializer (typeof(GameSettings));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Open)) {
			return serializer.Deserialize (stream) as GameSettings;
		}
	}

	/// <summary>
	/// Loads a game setting from an XML text.
	/// </summary>
	/// <returns>The game setting data.</returns>
	/// <param name="text">A text in XML format that contains the data that will be loaded.</param>
	public static GameSettings LoadFromText(string text)
	{
		var serializer = new XmlSerializer (typeof(GameSettings));
		return serializer.Deserialize (new StringReader (text)) as GameSettings;
	}
}
