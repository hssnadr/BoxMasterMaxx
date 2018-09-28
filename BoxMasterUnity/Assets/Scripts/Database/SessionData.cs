using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class SessionData
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
        /// </summary>
        public int timeoutScreenCount { get; set; }
        /// <summary>
        /// The time spent on the menu (in seconds).
        /// </summary>
        public int timeSpentOnMenu { get; set; }
        /// <summary>
        /// Total time spent on the game.
        /// </summary>
        public int timeSpentTotal { get; set; }
        /// <summary>
        /// Did the session finish on a time out ?
        /// </summary>
        public bool timeout { get; set; }
        /// <summary>
        /// Did the session finish on a debug-forced exit ?
        /// </summary>
        public bool debugExit { get; set; }
        /// <summary>
        /// In which game mode the session was played.
        /// </summary>
        public GameMode mode { get; set; }
        /// <summary>
        /// Score at the end of the session.
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// The precision given at the end of the session, value between 0.0 and 1.0.
        /// </summary>
        public float precision { get; set; }
        /// <summary>
        /// The speed given at the end of the session, value between 0.0 and 1.0.
        /// </summary>
        public float speed { get; set; }
        /// <summary>
        /// The value of the highest combo multiplier the players achieved during the session.
        /// </summary>
        public int highestComboMultiplier { get; set; }
    }
}
