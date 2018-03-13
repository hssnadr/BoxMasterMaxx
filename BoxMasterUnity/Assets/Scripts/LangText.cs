using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

[System.Serializable]
public struct LangTextEntry {
	[XmlAttribute("key")]
	public string key;
	[XmlAttribute("text")]
	public string text;

	public LangTextEntry (string key, string text)
	{
		this.key = key;
		this.text = text;
	}
}

[System.Serializable]
public class LangText {
	/// <summary>
	///The code ISO 639-1 of the language.
	/// </summary>
	public string code;

	[XmlArray("lang_text_entries")]
	[XmlArrayItem(typeof(LangTextEntry), ElementName = "text_entry")]
	public LangTextEntry[] arrayOfLangTextEntry;

	public LangText() {
	}

	public LangText(string code)
	{
		this.code = code;
	}

	public void Save(string path)
	{
		var serializer = new XmlSerializer (typeof(LangText));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Create)) 
		using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
		{
			xmlWriter.Formatting = Formatting.Indented;
			serializer.Serialize (xmlWriter, this);
		}
	}

	public static LangText Load(string path)
	{
		var serializer = new XmlSerializer (typeof(LangText));
		using (var stream = new FileStream (Path.Combine(Application.dataPath, path), FileMode.Open)) {
			return serializer.Deserialize (stream) as LangText;
		}
	}

	public static LangText LoadFromText(string text)
	{
		var serializer = new XmlSerializer (typeof(GameSettings));
		return serializer.Deserialize (new StringReader (text)) as LangText;
	}
}