using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class PlayerData : DataEntry
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
        /// X position of the setup hit. Can be null.
        /// </summary>
        public float? setupHitPositionX { get; set; }
        /// <summary>
        /// Y position of the setup hit. Can be null.
        /// </summary>
        public float? setupHitPositionY { get; set; }
        /// <summary>
        /// Z position of the setup hit. Can be null.
        /// </summary>
        public float? setupHitPositionZ { get; set; }

        public const string tableName = "players";

        public const string idString = "id";
        public const string sessionIdString = "session_id";
        public const string playerIndexString = "game_index";
        public const string setupHitPositionXString = "setup_hit_position_x";
        public const string setupHitPositionYString = "setup_hit_position_y";
        public const string setupHitPositionZString = "setup_hit_position_z";

        protected override DataEntry ToDataEntry(string item)
        {
            float setupHitPositionX;
            float setupHitPositionY;
            float setupHitPositionZ;
            var playerData = new PlayerData();
            playerData.id = int.Parse(GetDataValue(item, idString));
            playerData.sessionId = int.Parse(GetDataValue(item, sessionIdString));
            playerData.playerIndex = int.Parse(GetDataValue(item, playerIndexString));
            if (float.TryParse(GetDataValue(item, setupHitPositionXString), out setupHitPositionX))
                playerData.setupHitPositionX = setupHitPositionX;
            if (float.TryParse(GetDataValue(item, setupHitPositionYString), out setupHitPositionY))
                playerData.setupHitPositionY = setupHitPositionY;
            if (float.TryParse(GetDataValue(item, setupHitPositionZString), out setupHitPositionZ))
                playerData.setupHitPositionZ = setupHitPositionZ;
            return playerData;
        }

        public override string GetTableName()
        {
            return tableName;
        }

    }
}
