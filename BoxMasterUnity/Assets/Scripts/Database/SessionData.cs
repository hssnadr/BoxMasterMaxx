using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class SessionData : DataEntry
    {
        /// <summary>
        /// Index of the session in the database.
        /// </summary>
        [Field("id"), AutoIncrement, PrimaryKey]
        public int id { get; set; }
        /// <summary>
        /// The init file index.
        /// </summary>
        [Field("init_id")]
        public int initId { get; set; }
        /// <summary>
        /// Time when the session started.
        /// </summary>
        [Field("time")]
        public DateTime time { get; set; }
        /// <summary>
        /// Code of the language of the session.
        /// </summary>
        [Field("lang_code")]
        public string langCode { get; set; }
        /// <summary>
        /// How many times the timeout screen appeared during this session.
        /// Can be null.
        /// </summary>
        [Field("timeout_screen_count")]
        public int? timeoutScreenCount { get; set; }
        /// <summary>
        /// The time spent on the menu (in seconds).
        /// Can be null.
        /// </summary>
        [Field("time_spent_on_menu")]
        public int? timeSpentOnMenu { get; set; }
        /// <summary>
        /// Total time spent on the game.
        /// Can be null.
        /// </summary>
        [Field("time_spent_total")]
        public int? timeSpentTotal { get; set; }
        /// <summary>
        /// Did the session finish on a time out ?
        /// Can be null.
        /// </summary>
        [Field("timeout")]
        public bool? timeout { get; set; }
        /// <summary>
        /// Did the session finish on a debug-forced exit ?
        /// Can be null.
        /// </summary>
        [Field("debug_exit")]
        public bool? debugExit { get; set; }
        /// <summary>
        /// In which game mode the session was played.
        /// Can be null.
        /// </summary>
        [Field("game_mode")]
        public GameMode? gameMode { get; set; }
        /// <summary>
        /// Score at the end of the session.
        /// Can be null.
        /// </summary>
        [Field("score")]
        public int? score { get; set; }
        /// <summary>
        /// The precision given at the end of the session, value between 0.0 and 1.0.
        /// Can be ull.
        /// </summary>
        [Field("precision_rating")]
        public float? precisionRating { get; set; }
        /// <summary>
        /// The speed given at the end of the session, value between 0.0 and 1.0.
        /// Can be null.
        /// </summary>
        [Field("speed_rating")]
        public float? speedRating { get; set; }
        /// <summary>
        /// The value of the highest combo multiplier the players achieved during the session.
        /// Can be null.
        /// </summary>
        [Field("highest_combo_multiplier")]
        public int? highestComboMultiplier { get; set; }

        public const string name = "session";
        public const string tableName = "sessions";
        
        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public SessionData (int id, InitData init, DateTime time, string langCode,
            int timeoutScreenCount, int timeSpentOnMenu, int timeSpentTotal,
            bool timeout, bool debugExit, GameMode gameMode, int score,
            float precisionRating, float speedRating, int highestComboMultiplier)
        {
            this.id = id;
            initId = init.id;
            this.time = time;
            this.langCode = langCode;
            this.timeoutScreenCount = timeoutScreenCount;
            this.timeSpentOnMenu = timeSpentOnMenu;
            this.timeSpentTotal = timeSpentTotal;
            this.timeout = timeout;
            this.debugExit = debugExit;
            this.gameMode = gameMode;
            this.score = score;
            this.precisionRating = precisionRating;
            this.speedRating = speedRating;
            this.highestComboMultiplier = highestComboMultiplier;
        }

        public SessionData() { }
    }
}
