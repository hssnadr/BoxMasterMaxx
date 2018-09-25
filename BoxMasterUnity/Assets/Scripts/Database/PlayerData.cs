using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database {
    public class PlayerData
    {
        /// <summary>
        /// Index of the player in the database.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// The index of the player during the game. The value should be 0 (left player) or 1 (right player).
        /// </summary>
        [NotNull]
        public int playerIndex { get; set; }
        /// <summary>
        /// In which game mode the player played.
        /// </summary>
        [NotNull]
        public GameMode mode { get; set; }
        /// <summary>
        /// Index of the partner of the player if it was two player mode.
        /// </summary>
        [Indexed]
        public int partnerId { get; set; }
        /// <summary>
        /// Score of the player.
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// The precision of the player, value between 0.0 and 1.0.
        /// </summary>
        public float precision { get; set; }
        /// <summary>
        /// The speed of the player, value between 0.0 and 1.0.
        /// </summary>
        public float speed { get; set; }
        /// <summary>
        /// Time when the player has been saved in the database.
        /// </summary>
        [NotNull]
        public DateTime time { get; set; }
        /// <summary>
        /// The number of hits, successful or not.
        /// </summary>
        public int hitCount { get; set; }
        /// <summary>
        /// The number of successful hits.
        /// </summary>
        public int successfulHitCount { get; set; }
        /// <summary>
        /// The value of the highest combo multiplier the player achieved during the game.
        /// </summary>
        public int highestComboMultiplier { get; set; }
        /// <summary>
        /// The time spend on the menu (in seconds).
        /// </summary>
        public int timeSpentOnMenu { get; set; }
        /// <summary>
        /// Total time spent on the game.
        /// </summary>
        public int timeSpentTotal { get; set; }
        /// <summary>
        /// The player timed out.
        /// </summary>
        public bool timeout { get; set; }
        /// <summary>
        /// The init file index.
        /// </summary>
        public int initId { get; set; }
    }
}
