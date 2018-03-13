using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;

[System.Serializable]
public struct SerialPortSettings
{
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

	public SerialPortSettings (string name, int baudRate, int readTimeOut=0){
		this.name = name;
		this.baudRate = baudRate;
		this.readTimeOut = readTimeOut;
	}
}

[System.Serializable]
public struct LangApp
{
	[XmlAttribute("code")]
	/// <summary>
	/// The code ISO 639-1 of the language.
	/// </summary>
	public string code;
	[XmlAttribute("name")]
	/// <summary>
	/// The english name of the language.
	/// </summary>
	public string name;

	public LangApp (string code, string name="") {
		this.code = code;
		this.name = name;
	}
}

[System.Serializable]
public class GameSettings {
	/// <summary>
	/// The name of the application.
	/// </summary>
	public string name;
	/// <summary>
	/// Settings for the touch surface serial port
	/// </summary>
	[XmlElement("touch_surface_serial_port")]
	public SerialPortSettings touchSurfaceSerialPort;
	/// <summary>
	/// Settings for the led control serial port
	/// </summary>
	[XmlElement("led_control_serial_port")]
	public SerialPortSettings ledControlSerialPort;
	/// <summary>
	/// Min value to detect impact
	/// </summary>
	[XmlElement("impact_threshold")]
	public int impactThreshold;
	/// <summary>
	/// Time until the application returns to the home screen.
	/// </summary>
	[XmlElement("time_out")]
	public float timeOut;
	/// <summary>
	/// All of the languages available for the translation of the application.
	/// </summary>
	[XmlArray("lang_app_available")]
	[XmlArrayItem(typeof(LangApp), ElementName = "lang_app")]
	[SerializeField]
	public LangApp[] langAppAvailable;

	/// <summary>
	/// Code of the enabled languages in priority order of appearance in the menus
	/// </summary>
	[XmlArray("lang_app_enable")]
	[XmlArrayItem(typeof(string), ElementName = "code")]
	[SerializeField]
	public string[] langAppEnableCodes; 

	/// <summary>
	/// Enabled languages in priority order of appearance in the menus
	/// </summary>
	protected LangApp[] _langAppEnable;

	[XmlIgnore]
	public IList<LangApp> langAppEnable {
		get {
			return _langAppEnable.ToList().AsReadOnly ();
		}
	}

	/// <summary>
	/// Gets the default language, which is the first of the list.
	/// </summary>
	/// <value>The default language.</value>
	[XmlIgnore]
	public LangApp defaultLanguage {
		get {
			return _langAppEnable [0];
		}
	}


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
			var gameSettings = serializer.Deserialize (stream) as GameSettings;
			gameSettings._langAppEnable = new LangApp[gameSettings.langAppEnableCodes.Length];
			for (int i = 0; i < gameSettings.langAppEnableCodes.Length; i++) {
				var code = gameSettings.langAppEnableCodes [i];
				var langApp = gameSettings.langAppAvailable.First (x => x.code == code);
				gameSettings._langAppEnable [i] = langApp;
			}
			return gameSettings;
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
