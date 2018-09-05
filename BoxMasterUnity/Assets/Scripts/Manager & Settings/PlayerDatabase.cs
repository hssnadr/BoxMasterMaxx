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
using System.Threading;

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
    /// Time when the player has been saved in the database.
    /// </summary>
    public DateTime date;
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
    public PlayerData (int index, int playerIndex, int partnerIndex, GameMode mode, DateTime date, int score, List<string> answers)
    {
        this.index = index;
        this.playerIndex = playerIndex;
        this.partnerIndex = partnerIndex;
        this.mode = mode;
        this.date = date;
        this.score = score;
        this.answers = answers;
    }
}

/*
public struct PlayerRank
{
    public PlayerData player;

    public int rank;

    public PlayerRank (PlayerData player, int rank)
    {
        this.player = player;
        this.rank = rank;
    }
}
*/

public class PlayerDatabase
{
    /// <summary>
    /// An array of player data.
    /// </summary>
    public List<PlayerData> players = new List<PlayerData>();
    /// <summary>
    /// Thread list.
    /// </summary>
    public List<Thread> _threads = new List<Thread>();
    /// <summary>
    /// Thread lock.
    /// </summary>
    private readonly System.Object _lock = new System.Object();

    private const string dateFormat = "MM/dd/yyyy HH:mm:ss";

    /// <summary>
    /// Save the data to a csv file.
    /// </summary>
    /// <param name="path">The path where the csv file will be saved.</param>
    public void SaveAll(string path)
    {
        var thread = new Thread(() => ThreadSaveAll(path));
        _threads.Add(new Thread(() => ThreadSaveAll(path)));
        thread.Start();
    }

    /// <summary>
    /// Appends a list of players to the csv database.
    /// </summary>
    /// <param name="path">The path of the csv database</param>
    /// <param name="players">The list of players</param>
    public void Save(string path, List<PlayerData> players)
    {
        var thread = new Thread(() => ThreadSave(path, players));
        _threads.Add(new Thread(() => ThreadSave(path, players)));
        thread.Start();
    }

    private void ThreadSaveAll(string path)
    {
        lock (_lock)
        {
            try
            {
                using (var writer = new StreamWriter(path, false))
                {
                    string typeString = "Index,Player,PartnerIndex,Mode,Score";
                    foreach (var surveyQuestion in GameManager.instance.gameSettings.surveySettings.surveyQuestions)
                    {
                        typeString += "," + surveyQuestion.key;
                    }
                    writer.WriteLine(typeString);

                    foreach (var player in players)
                        writer.WriteLine(GetPlayerString(player));

                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    private void ThreadSave(string path, List<PlayerData> players)
    {
        lock (_lock)
        {
            try
            {
                using (var writer = new StreamWriter(path, append: true))
                {

                    for (int i = 0; i < players.Count; i++)
                        writer.WriteLine(GetPlayerString(players[i]));

                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    /// </summary>
    /// <returns>The player database.</returns>
    /// <param name="path">The path here the csv file is located.</param>
    public static PlayerDatabase Load(string path)
    {
        try
        {
            string file = "";
            using (var stream = new StreamReader(File.OpenRead(path)))
            {
                file = stream.ReadToEnd();
                return LoadFromText(file);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return new PlayerDatabase();
        }
    }

    /// <summary>
    /// Loads a player database from an csv text.
    /// </summary>
    /// <param name="text">The player database.</param>
    /// <returns>A text in csv format that contains the player info.</returns>
    public static PlayerDatabase LoadFromText(string text)
    {
        string[] fileLines = text.Split(System.Environment.NewLine.ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        List<string[]> csvList = new List<string[]>();
        PlayerDatabase database = new PlayerDatabase();

        foreach (string fileLine in fileLines)
        {
            csvList.Add(fileLine.Split(",".ToCharArray(), System.StringSplitOptions.None));
        }
        var playerData = new List<PlayerData>();
        for (int i = 1; i < csvList.Count; i++)
        {
            Int32 playerIndex = Int32.Parse(csvList[i][1]);
            Int32 index = Int32.Parse(csvList[i][0]);
            int partnerIndex = string.IsNullOrEmpty(csvList[i][2]) ? 0 : Int32.Parse(csvList[i][2]);
            GameMode mode = (GameMode)int.Parse(csvList[i][3]);
            Int32 score = Int32.Parse(csvList[i][4]);
            DateTime date = DateTime.ParseExact(csvList[i][5],
                dateFormat,
                System.Globalization.CultureInfo.CurrentCulture,
                System.Globalization.DateTimeStyles.None);

            List <string> answers = new List<string>();
            for (int j = 6; j < csvList[i].Length; j++)
                answers.Add(csvList[i][j]);

            var player = new PlayerData(index, playerIndex, partnerIndex, mode, date, score, answers);

            playerData.Add(player);
        }
        database.players = playerData;
        return database;
    }

#if UNITY_EDITOR
    public void AddRandomPlayers(int number)
    {
        int index = players.Count() + 1;
        for (int i = 0; i < number; i++)
        {
            players.Add(new PlayerData(index + i,
                UnityEngine.Random.Range(0, 2),
                0,
                (GameMode)UnityEngine.Random.Range(0, 2),
                DateTime.Now,
                UnityEngine.Random.Range(0, 1000000),
                new List<string>() { "", "", "", "" }));
        }
    }
#endif

    private string GetPlayerString(PlayerData player)
    {
        int questionCount = GameManager.instance.gameSettings.surveySettings.surveyQuestions.Length;
        string playerString = "";
        playerString = String.Format("{0},{1},{2},{3},{4},{5}",
            player.index,
            player.playerIndex,
            player.partnerIndex == 0 ? "" : player.partnerIndex.ToString(),
            (int)player.mode,
            player.score,
            player.date.ToString(dateFormat)
            );
        if (player.answers.Count == questionCount)
        {
            foreach (string answer in player.answers)
            {
                playerString += "," + answer;
            }
        }
        else
            playerString += new string(',', questionCount);
        return playerString;
    }

    public int GetNumberOfPlayers(GameMode gameMode)
    {
        return players
            .Where(x => x.mode == gameMode)
            .Count();
    }

    public int GetRank(GameMode gameMode, int score)
    {
        return players
            .Where(x => x.mode == gameMode)
            .Select(x => x.score)
            .OrderByDescending(x => x)
            .Select((x, i) => new { score = x, rank = i + 1 })
            .First(x => x.score == score)
            .rank;
    }

    public int GetBestScore(GameMode gameMode)
    {
        return players.Where(x => x.mode == gameMode).Select(x => x.score).Max(x => x);
    }
}
