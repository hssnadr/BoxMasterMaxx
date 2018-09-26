using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class PlayerData
    {
        /// <summary>
        /// Index of the player in the database.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// The session index.
        /// </summary>
        [Indexed, NotNull]
        public int sessionId { get; set; }
        /// <summary>
        /// The index of the player during the game. The value should be 0 (left player) or 1 (right player).
        /// </summary>
        [NotNull]
        public int playerIndex { get; set; }
        /// <summary>
        /// X position of the setup hit.
        /// </summary>
        public float setupHitPositionX { get; set; }
        /// <summary>
        /// Y position of the setup hit.
        /// </summary>
        public float setupHitPositionY { get; set; }
        /// <summary>
        /// Z position of the setup hit.
        /// </summary>
        public float setupHitPositionZ { get; set; }
    }
}
