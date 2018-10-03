using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class HitData : DataEntry
    {
        /// <summary>
        /// Index of the hit.
        /// </summary>
        [Field("id")]
        public int? id { get; set; }
        /// <summary>
        /// The index of the player that caused the hit.
        /// </summary>
        [Field("player_id")]
        public int playerId { get; set; }
        /// <summary>
        /// Time when the hit occured.
        /// </summary>
        [Field("time")]
        public DateTime time { get; set; }
        /// <summary>
        /// X position of the hit.
        /// </summary>
        [Field("position_x")]
        public float positionX { get; set; }
        /// <summary>
        /// Y position of the hit.
        /// </summary>
        [Field("position_y")]
        public float positionY { get; set; }
        /// <summary>
        /// Was the hit successful ?
        /// </summary>
        [Field("successful")]
        public bool successful { get; set; }
        /// <summary>
        /// X position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_x")]
        public float targetCenterX { get; set; }
        /// <summary>
        /// Y position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_y")]
        public float targetCenterY { get; set; }
        /// <summary>
        /// Z position of the center of the target that was hit.
        /// </summary>
        [Field("target_center_z")]
        public float targetCenterZ { get; set; }
        /// <summary>
        /// X position of the target.
        /// </summary>
        [Field("target_speed_vector_x")]
        public float targetSpeedVectorX { get; set; }
        /// <summary>
        /// Y position of the target.
        /// </summary>
        [Field("target_speed_vector_y")]
        public float targetSpeedVectorY { get; set; }
        /// <summary>
        /// Z position of the target.
        /// </summary>
        [Field("target_speed_vector_z")]
        public float targetSpeedVectorZ { get; set; }

        public Vector2 position
        {
            get
            {
                return new Vector2(positionX, positionY);
            }
            set
            {
                positionX = value.x;
                positionY = value.y;
            }
        }

        public Vector3 targetCenter
        {
            get
            {
                return new Vector3(targetCenterX, targetCenterY, targetCenterZ);
            }
            set
            {
                targetCenterX = value.x;
                targetCenterY = value.y;
                targetCenterZ = value.z;
            }
        }

        public Vector3 targetSpeedVector
        {
            get
            {
                return new Vector3(targetSpeedVectorX, targetSpeedVectorY, targetSpeedVectorZ);
            }
            set
            {
                targetSpeedVectorX = value.x;
                targetSpeedVectorY = value.y;
                targetSpeedVectorZ = value.z;
            }
        }

        public const string name = "hit";
        public const string tableName = "hits";

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public HitData(int id, PlayerData player, DateTime time, Vector2 position, bool successful, Vector3 targetCenter, Vector3 targetSpeedVector)
        {
            this.id = id;
            this.playerId = player.id.Value;
            this.time = time;
            this.position = position;
            this.successful = successful;
            this.targetCenter = targetCenter;
            this.targetSpeedVector = targetSpeedVector;
        }

        public HitData() { }
    }
}
