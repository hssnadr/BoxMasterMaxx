using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[System.Serializable]
public class QuestionAnswerKeys
{
    /// <summary>
    /// The text key of a question of the survey.
    /// </summary>
    [XmlAttribute("key")]
    public string questionKey;
    /// <summary>
    /// The corresponding text key of the answer.
    /// </summary>
    [XmlAttribute("answer")]
    public string answerKey;

    public QuestionAnswerKeys(string questionKey, string answerKey)
    {
        this.questionKey = questionKey;
        this.answerKey = answerKey;
    }
}

[System.Serializable]
public struct PlayerData
{
    /// <summary>
    /// Whether the player was the player 
    /// </summary>
    [XmlElement("player_index")]
    public int playerIndex;
    /// <summary>
    /// The index of the player in the database.
    /// </summary>
    [XmlElement("index")]
    public int index;
    /// <summary>
    /// Index of the partner of the player if it was two player mode.
    /// </summary>
    [XmlElement("partner_index")]
    public int partnerIndex;
    /// <summary>
    /// In which game mode the player played.
    /// </summary>
    [XmlElement("mode")]
    public GameMode mode;
    /// <summary>
    /// Score of the player.
    /// </summary>
    [XmlElement("score")]
    public int score;
    /// <summary>
    /// The answers to the survey.
    /// </summary>
    [XmlArray("survey")]
    [XmlArrayItem(typeof(QuestionAnswerKeys), ElementName = "question")]
    [SerializeField]
    public QuestionAnswerKeys[] questionAnswerKeys;
}

public class PlayerDatabase
{
    /// <summary>
    /// Save the data to an XML file.
    /// </summary>
    /// <param name="path">The path where the XML file will be saved.</param>
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(PlayerDatabase));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Create))
        using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
        {
            xmlWriter.Formatting = Formatting.Indented;
            serializer.Serialize(xmlWriter, this);
        }
    }

    /// </summary>
    /// <returns>The player database.</returns>
    /// <param name="path">The path here the XML file is located.</param>
    public static PlayerDatabase Load(string path)
    {
        var serializer = new XmlSerializer(typeof(PlayerDatabase));
        using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Open))
        {
            return serializer.Deserialize(stream) as PlayerDatabase;
        }
    }

    /// <summary>
    /// Loads a player database from an XML text.
    /// </summary>
    /// <param name="text">The player database.</param>
    /// <returns>A text in XML format that contains the player info.</returns>
    public static PlayerDatabase LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(PlayerDatabase));
        return serializer.Deserialize(new StringReader(text)) as PlayerDatabase;
    }
}
