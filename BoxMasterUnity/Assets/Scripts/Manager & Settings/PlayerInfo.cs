using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityScript.Steps;
using System.Linq;


[System.Serializable]
public struct PlayerData
{
    /// <summary>
    /// Whether the player was the player 1 or 2
    /// </summary>
    public int playerIndex;
    /// <summary>
    /// The index of the player in the database.
    /// </summary>
    public int index;
    /// <summary>
    /// Index of the partner of the player if it was two player mode.
    /// </summary>
    public int partnerIndex;
    /// <summary>
    /// In which game mode the player played.
    /// </summary>
    public GameMode mode;
    /// <summary>
    /// Score of the player.
    /// </summary>
    public int score;
    /// <summary>
    /// The answers to the survey.
    /// </summary>
    public List<string> answers;

    /// <summary>
    /// Creates a player data.
    /// </summary>
    /// <param name="index">The corresponding text key of the answer.</param>
    /// <param name="playerIndex">Whether the player was the player 1 or 2.</param>
    /// <param name="partnerIndex">Index of the partner of the player if it was two player mode.</param>
    /// <param name="mode"> In which game mode the player played.</param>
    /// <param name="score">Score of the player.</param>
    /// <param name="answers">The answers to the survey.</param>
    public PlayerData (int index, int playerIndex, int partnerIndex, GameMode mode, int score, List<string> answers)
    {
        this.index = index;
        this.playerIndex = playerIndex;
        this.partnerIndex = partnerIndex;
        this.mode = mode;
        this.score = score;
        this.answers = answers;
    }
}

public class PlayerDatabase
{
    /// <summary>
    /// An array of player data.
    /// </summary>
    public List<PlayerData> playerData;
    /// <summary>
    /// Save the data to an XML file.
    /// </summary>
    /// <param name="path">The path where the XML file will be saved.</param>
    public void Save(string path)
    {
        using (var writer = new StreamWriter(path, false))
        {
            string typeString = "Index,Player,PartnerIndex,Mode,Score";
            foreach (var surveyQuestion in GameManager.instance.gameSettings.surveySettings.surveyQuestions)
            {
                typeString += "," + surveyQuestion.key;
            }
            writer.WriteLine(typeString);

            for (int i = 0; i < playerData.Count; i++)
            {
                string playerString = "";
                var player = playerData[i];
                playerString = String.Format("{0},{1},{2},{3},{4}",
                    player.index,
                    player.playerIndex,
                    player.partnerIndex == 0 ? "" : player.partnerIndex.ToString(),
                    player.mode
                    );
                foreach (string answer in player.answers)
                {
                    playerString += "," + answer;
                }
                writer.WriteLine(playerString);
            }

            writer.Flush();
            writer.Close();
        }
    }

    /// </summary>
    /// <returns>The player database.</returns>
    /// <param name="path">The path here the XML file is located.</param>
    public static PlayerDatabase Load(string path)
    {
        string file = "";
        using (var stream = new StreamReader(File.OpenRead(path)))
        {
            file = stream.ReadToEnd();
        }
        return LoadFromText(file);
    }

    /// <summary>
    /// Loads a player database from an XML text.
    /// </summary>
    /// <param name="text">The player database.</param>
    /// <returns>A text in XML format that contains the player info.</returns>
    public static PlayerDatabase LoadFromText(string text)
    {
        string[] fileLines = text.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        List<string[]> csvList = new List<string[]>();
        PlayerDatabase database = new PlayerDatabase();

        foreach (string fileLine in fileLines)
        {
            csvList.Add(fileLine.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries));
        }
        var playerData = new List<PlayerData>();
        if (csvList[0] != null)
        {
            for (int j = 1; j < csvList[0].Length; j++)
            {
                Int32 playerIndex = Int32.Parse(csvList[1][j]);
                Int32 index = Int32.Parse(csvList[0][j]);
                Int32 partnerIndex = Int32.Parse(csvList[2][j]);
                GameMode mode = (GameMode)Int32.Parse(csvList[3][j]);
                Int32 score = Int32.Parse(csvList[4][j]);
                List<string> answers = new List<string>();
                for (int i = 5; i < csvList.Count; i++)
                    answers.Add(csvList[i][j]);

                var player = new PlayerData(index, playerIndex, partnerIndex, mode, score, answers);

                playerData.Add(player);
            }
        }
        database.playerData = playerData;
        return database;
    }
}
