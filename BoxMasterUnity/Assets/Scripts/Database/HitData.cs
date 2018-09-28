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
    }
}
