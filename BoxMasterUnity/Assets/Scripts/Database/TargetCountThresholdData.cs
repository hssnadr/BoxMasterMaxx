using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class TargetCountThresholdData : DataEntry
    {
        /// <summary>
        /// The index of the init file.
        /// </summary>
        public int initId { get; set; }
        /// <summary>
        /// Index of the threshold.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Threshold for increasing the number of targets.
        /// </summary>
        public int countThreshold { get; set; }

        public const string tableName = "target_count_threshold";

        public const string initIdString = "init_id";
        public const string idString = "id";
        public const string countThresholdString = "count_threshold";

        protected override DataEntry ToDataEntry(string item)
        {
            var targetCountThresholdData = new TargetCountThresholdData();
            targetCountThresholdData.initId = int.Parse(GetDataValue(item, initIdString));
            targetCountThresholdData.id = int.Parse(GetDataValue(item, idString));
            targetCountThresholdData.countThreshold = int.Parse(GetDataValue(item, countThresholdString));
            return targetCountThresholdData;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public override string ToString()
        {
            return string.Format(culture, "TargetCountThreshold = [id = {0}, init_id = {1}, count_threshold = {2}]", id, initId, countThreshold);
        }
    }
}
