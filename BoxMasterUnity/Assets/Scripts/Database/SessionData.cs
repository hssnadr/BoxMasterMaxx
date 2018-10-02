using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class SessionData : DataEntry
    {
        /// <summary>
        /// Index of the session in the database.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// The init file index.
        /// </summary>
        public int initId { get; set; }
        /// <summary>
        /// Time when the session started.
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// Code of the language of the session.
        /// </summary>
        public string langCode { get; set; }
        /// <summary>
        /// How many times the timeout screen appeared during this session.
        /// Can be null.
        /// </summary>
        public int? timeoutScreenCount { get; set; }
        /// <summary>
        /// The time spent on the menu (in seconds).
        /// Can be null.
        /// </summary>
        public int? timeSpentOnMenu { get; set; }
        /// <summary>
        /// Total time spent on the game.
        /// Can be null.
        /// </summary>
        public int? timeSpentTotal { get; set; }
        /// <summary>
        /// Did the session finish on a time out ?
        /// Can be null.
        /// </summary>
        public bool? timeout { get; set; }
        /// <summary>
        /// Did the session finish on a debug-forced exit ?
        /// Can be null.
        /// </summary>
        public bool? debugExit { get; set; }
        /// <summary>
        /// In which game mode the session was played.
        /// Can be null.
        /// </summary>
        public GameMode? gameMode { get; set; }
        /// <summary>
        /// Score at the end of the session.
        /// Can be null.
        /// </summary>
        public int? score { get; set; }
        /// <summary>
        /// The precision given at the end of the session, value between 0.0 and 1.0.
        /// Can be ull.
        /// </summary>
        public float? precisionRating { get; set; }
        /// <summary>
        /// The speed given at the end of the session, value between 0.0 and 1.0.
        /// Can be null.
        /// </summary>
        public float? speedRating { get; set; }
        /// <summary>
        /// The value of the highest combo multiplier the players achieved during the session.
        /// Can be null.
        /// </summary>
        public int? highestComboMultiplier { get; set; }

        public const string tableName = "sessions";

        public const string idString = "id";
        public const string initIdString = "init_id";
        public const string timeString = "time";
        public const string langCodeString = "lang_code";
        public const string timeoutScreenCountString = "timeout_screen_count";
        public const string timeSpentOnMenuString = "time_spent_on_menu";
        public const string timeSpentTotalString = "time_spent_total";
        public const string timeoutString = "timeout";
        public const string debugExitString = "debug_exit";
        public const string gameModeString = "game_mode";
        public const string precisionRatingString = "precision_rating";
        public const string speedRatingString = "speed_rating";
        public const string highestComboMultiplierString = "higher_combo_multiplier";

        protected override DataEntry ToDataEntry(string item)
        {
            int timeoutScreenCount, timeSpentOnMenu, timeSpentTotal, timeout, debugExit, mode, highestComboMultiplier;
            float precision, speed;
            var sessionData = new SessionData();
            sessionData.id = int.Parse(GetDataValue(item, idString));
            sessionData.initId = int.Parse(GetDataValue(item, initIdString));
            sessionData.time = DateTime.Parse(GetDataValue(item, timeString));
            sessionData.langCode = GetDataValue(item, langCodeString);
            if (int.TryParse(GetDataValue(item, timeoutScreenCountString), out timeoutScreenCount))
                sessionData.timeoutScreenCount = timeoutScreenCount;
            if (int.TryParse(GetDataValue(item, timeSpentOnMenuString), out timeSpentOnMenu))
                sessionData.timeSpentOnMenu = timeSpentOnMenu;
            if (int.TryParse(GetDataValue(item, timeSpentTotalString), out timeSpentTotal))
                sessionData.timeSpentTotal = timeSpentTotal;
            if (int.TryParse(GetDataValue(item, timeoutString), out timeout))
                sessionData.timeout = timeout != 0;
            if (int.TryParse(GetDataValue(item, debugExitString), out debugExit))
                sessionData.debugExit = debugExit != 0;
            if (int.TryParse(GetDataValue(item, gameModeString), out mode))
                sessionData.gameMode = (GameMode)mode;
            if (float.TryParse(GetDataValue(item, precisionRatingString), out precision))
                sessionData.precisionRating = precision;
            if (float.TryParse(GetDataValue(item, speedRatingString), out speed))
                sessionData.speedRating = speed;
            if (int.TryParse(GetDataValue(item, highestComboMultiplierString), out highestComboMultiplier))
                sessionData.highestComboMultiplier = highestComboMultiplier;
            return sessionData;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public override string ToString()
        {
            return string.Format("Session = [id = {0}, init_id = {1}, time = {2}, langCode = {3}, timeout_screen_count = {4}, time_spent_total = {5}, time_spent_on_menu = {6}, timeout = {7}, debug_exit = {8}, game_mode = {9}, precision_rating = {10}, speed_rating = {11}, highest_combo_multiplier = {12}]",
                id, initId, time, langCode, timeoutScreenCount, timeSpentOnMenu, timeSpentTotalString, timeout, debugExit, gameMode, precisionRating, speedRating, highestComboMultiplier);
        }
    }
}
