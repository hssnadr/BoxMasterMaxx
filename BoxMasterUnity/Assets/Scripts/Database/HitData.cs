using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class HitData
    {
        /// <summary>
        /// Index of the hit.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// The index of the player that caused the hit.
        /// </summary>
        [Indexed]
        public int playerId { get; set; }
        /// <summary>
        /// Time when the hit occured.
        /// </summary>
        [NotNull]
        public DateTime time { get; set; }
        /// <summary>
        /// X position of the hit.
        /// </summary>
        [NotNull]
        public float positionX { get; set; }
        /// <summary>
        /// Y position of the hit.
        /// </summary>
        [NotNull]
        public float positionY { get; set; }
        /// <summary>
        /// Was the hit successful ?
        /// </summary>
        [NotNull]
        public bool successful { get; set; }
        /// <summary>
        /// X position of the center of the target that was hit.
        /// </summary>
        [NotNull]
        public float targetCenterX { get; set; }
        /// <summary>
        /// Y position of the center of the target that was hit.
        /// </summary>
        [NotNull]
        public float targetCenterY { get; set; }
        /// <summary>
        /// Z position of the center of the target that was hit.
        /// </summary>
        [NotNull]
        public float targetCenterZ { get; set; }
        /// <summary>
        /// X position of the target.
        /// </summary>
        [NotNull]
        public float targetSpeedVectorX { get; set; }
        /// <summary>
        /// Y position of the target.
        /// </summary>
        [NotNull]
        public float targetSpeedVectorY { get; set; }
        /// <summary>
        /// Z position of the target.
        /// </summary>
        [NotNull]
        public float targetSpeedVectorZ { get; set; }
    }
}
