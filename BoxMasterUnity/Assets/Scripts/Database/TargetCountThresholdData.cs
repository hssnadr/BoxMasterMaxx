using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class TargetCountThresholdData
    {
        /// <summary>
        /// The index of the init file.
        /// </summary>
        [Indexed, NotNull]
        public int initId { get; set; }
        /// <summary>
        /// Index of the threshold.
        /// </summary>
        [Indexed, NotNull]
        public int id { get; set; }
        /// <summary>
        /// Threshold for increasing the number of targets.
        /// </summary>
        [NotNull]
        public int countThreshold { get; set; }
    }
}
