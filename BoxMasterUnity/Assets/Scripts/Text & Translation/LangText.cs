// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;

[System.Serializable]
public struct LangTextEntry
{
    [XmlAttribute("key")]
    public string key;
    [XmlText]
    public string text;

    public LangTextEntry(string key, string text)
    {
        this.key = key;
        this.text = text;
    }
}

/// <summary>
/// Contains all the text of a specific language. Is loaded from an XML files.
/// </summary>
[System.Serializable]
public class LangText
{
    /// <summary>
    /// The code ISO 639-1 of the language.
    /// </summary>
    public string code;

    /// <summary>
    /// All the text entries of a specific language.
    /// </summary>
    [XmlArray("lang_text_entries")]
    [XmlArrayItem(typeof(LangTextEntry), ElementName = "text_entry")]
    public LangTextEntry[] arrayOfLangTextEntry;

    public LangText()
    {
    }

    /// <summary>
    /// Instantiates a LangText from a code ISO 639-1.
    /// </summary>
    /// <param name="code">The code ISO 639-1 of the language.</param>
    public LangText(string code)
    {
        this.code = code;
    }

    /// <summary>
    /// Save the data to an XML file.
    /// </summary>
    /// <param name="path">The path where the XML file will be saved.</param>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(LangText));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Create))
        using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
        {
            xmlWriter.Formatting = Formatting.Indented;
            serializer.Serialize(xmlWriter, this);
        }
    }

    /// <summary>
    /// Loads a lang text data from the XML file at the specified path.
    /// </summary>
    /// <returns>The lang text data.</returns>
    /// <param name="path">The path here the XML file is located.</param>
    public static LangText Load(string path)
    {
        var serializer = new XmlSerializer(typeof(LangText));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Open))
        {
            return serializer.Deserialize(stream) as LangText;
        }
    }


    /// <summary>
    /// Loads a lang text from an XML text.
    /// </summary>
    /// <returns>The lang setting data.</returns>
    /// <param name="text">A text in XML format that contains the data that will be loaded.</param>
    public static LangText LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(GameSettings));
        return serializer.Deserialize(new StringReader(text)) as LangText;
    }
}