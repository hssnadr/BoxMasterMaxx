using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class TargetCountThresholdData : DataEntry
    {
        /// <summary>
        /// The index of the init file.
        /// </summary>
        [Field("init_id")]
        public int initId { get; set; }
        /// <summary>
        /// Index of the threshold.
        /// </summary>
        [Field("id")]
        public int id { get; set; }
        /// <summary>
        /// Threshold for increasing the number of targets.
        /// </summary>
        [Field("count_threshold")]
        public int countThreshold { get; set; }

        public const string name = "target_count_threshold";
        public const string tableName = "target_count_thresholds";

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public TargetCountThresholdData(int id, int initId, int countThreshold)
        {
            this.id = id;
            this.initId = initId;
            this.countThreshold = countThreshold;
        }

        public TargetCountThresholdData() { }
    }
}
