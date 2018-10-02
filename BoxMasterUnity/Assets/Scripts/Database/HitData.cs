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
        public int id { get; set; }
        /// <summary>
        /// The index of the player that caused the hit.
        /// </summary>
        public int playerId { get; set; }
        /// <summary>
        /// Time when the hit occured.
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// X position of the hit.
        /// </summary>
        public float positionX { get; set; }
        /// <summary>
        /// Y position of the hit.
        /// </summary>
        public float positionY { get; set; }
        /// <summary>
        /// Was the hit successful ?
        /// </summary>
        public bool successful { get; set; }
        /// <summary>
        /// X position of the center of the target that was hit.
        /// </summary>
        public float targetCenterX { get; set; }
        /// <summary>
        /// Y position of the center of the target that was hit.
        /// </summary>
        public float targetCenterY { get; set; }
        /// <summary>
        /// Z position of the center of the target that was hit.
        /// </summary>
        public float targetCenterZ { get; set; }
        /// <summary>
        /// X position of the target.
        /// </summary>
        public float targetSpeedVectorX { get; set; }
        /// <summary>
        /// Y position of the target.
        /// </summary>
        public float targetSpeedVectorY { get; set; }
        /// <summary>
        /// Z position of the target.
        /// </summary>
        public float targetSpeedVectorZ { get; set; }

        public const string name = "hit";
        public const string tableName = "hits";

        public const string idString = "id";
        public const string playerIdString = "player_id";
        public const string timeString = "time";
        public const string positionXString = "position_x";
        public const string positionYString = "position_y";
        public const string successfulString = "successful";
        public const string targetCenterXString = "target_center_x";
        public const string targetCenterYString = "target_center_y";
        public const string targetCenterZString = "target_center_z";
        public const string targetSpeedVectorXString = "target_speed_vector_x";
        public const string targetSpeedVectorYString = "target_speed_vector_y";
        public const string targetSpeedVectorZString = "target_speed_vector_z";

        protected override DataEntry ToDataEntry(string item)
        {
            var hitData = new HitData();
            hitData.id = int.Parse(GetDataValue(item, idString));
            hitData.playerId = int.Parse(GetDataValue(item, playerIdString));
            hitData.time = DateTime.Parse(GetDataValue(item, timeString));
            hitData.positionX = float.Parse(GetDataValue(item, positionXString), culture);
            hitData.positionY = float.Parse(GetDataValue(item, positionYString), culture);
            hitData.successful = int.Parse(GetDataValue(item, successfulString)) != 0;
            hitData.targetCenterX = float.Parse(GetDataValue(item, targetCenterXString), culture);
            hitData.targetCenterY = float.Parse(GetDataValue(item, targetCenterYString), culture);
            hitData.targetCenterZ = float.Parse(GetDataValue(item, targetCenterZString), culture);
            hitData.targetSpeedVectorX = float.Parse(GetDataValue(item, targetSpeedVectorXString), culture);
            hitData.targetSpeedVectorY = float.Parse(GetDataValue(item, targetSpeedVectorYString), culture);
            hitData.targetSpeedVectorZ = float.Parse(GetDataValue(item, targetSpeedVectorZString), culture);
            return hitData;
        }

        internal override WWWForm GetForm()
        {
            var form = new WWWForm();
            return form;
        }

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public override string ToString()
        {
            return string.Format(culture, "Hit = [id = {0}, player_id = {1}, time = {2}, position_x = {3}, position_y = {4}, successful = {5}, target_center_x = {6}, target_center_y = {7}, target_center_z = {8}, target_speed_vector_x = {9}, target_speed_vector_y = {10}, target_speed_vector_z = {11}]",
                id, playerId, time, positionX, positionY, successful, targetCenterX, targetCenterY, targetCenterZ, targetSpeedVectorX, targetSpeedVectorY, targetSpeedVectorZ);
        }
    }
}
