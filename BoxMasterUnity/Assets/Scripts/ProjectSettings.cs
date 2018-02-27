using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;

public struct SerialPortSettings
{
	public string name;
	public int baudRate;
}

public class ProjectSettings : MonoBehaviour {
	public SerialPortSettings touchSurfaceSerialPort;
	public SerialPortSettings ledControlSerialPort;

	public void Save(string path)
	{
		var serializer = new XmlSerializer (typeof(ProjectSettings));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Create)) 
		using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
		{
			xmlWriter.Formatting = Formatting.Indented;
			serializer.Serialize (xmlWriter, this);
		}
	}

	public static ProjectSettings Load(string path)
	{
		var serializer = new XmlSerializer (typeof(ProjectSettings));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Open)) {
			return serializer.Deserialize (stream) as ProjectSettings;
		}
	}

	public static ProjectSettings LoadFromText(string text)
	{
		var serializer = new XmlSerializer (typeof(ProjectSettings));
		return serializer.Deserialize (new StringReader (text)) as ProjectSettings;
	}
}
