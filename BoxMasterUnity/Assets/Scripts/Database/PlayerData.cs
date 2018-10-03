using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class PlayerData : DataEntry
    {
        /// <summary>
        /// Index of the player in the database.
        /// </summary>
        [Field("id"), AutoIncrement, PrimaryKey]
        public int id { get; set; }
        /// <summary>
        /// The session index.
        /// </summary>
        [Field("session_id")]
        public int sessionId { get; set; }
        /// <summary>
        /// The index of the player during the game. The value should be 0 (left player) or 1 (right player).
        /// </summary>
        [Field("game_index")]
        public int playerIndex { get; set; }
        /// <summary>
        /// X position of the setup hit. Can be null.
        /// </summary>
        [Field("setup_hit_position_x")]
        public float? setupHitPositionX { get; set; }
        /// <summary>
        /// Y position of the setup hit. Can be null.
        /// </summary>
        [Field("setup_hit_position_y")]
        public float? setupHitPositionY { get; set; }
        /// <summary>
        /// Z position of the setup hit. Can be null.
        /// </summary>
        [Field("setup_hit_position_z")]
        public float? setupHitPositionZ { get; set; }

        public Vector3? setupHitPosition
        {
            get
            {
                if (setupHitPositionX.HasValue && setupHitPositionY.HasValue && setupHitPositionZ.HasValue)
                    return new Vector3(
                        setupHitPositionX.Value,
                        setupHitPositionY.Value,
                        setupHitPositionZ.Value
                        );
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    setupHitPositionX = value.Value.x;
                    setupHitPositionY = value.Value.y;
                    setupHitPositionZ = value.Value.z;
                }
            }
        }

        public const string name = "player";
        public const string tableName = "players";

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public PlayerData (int id, SessionData session, int game_index, Vector3 setupHitPosition)
        {
            this.id = id;
            this.sessionId = session.id;
            this.setupHitPosition = setupHitPosition;
        }

        public PlayerData () { }
    }
}
