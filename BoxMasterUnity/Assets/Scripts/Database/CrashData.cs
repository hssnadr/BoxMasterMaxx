using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class CrashData : DataEntry
    {
        /// <summary>
        /// Index of the crash data.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Time of the crash.
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// Duration of the crash (in seconds). Can be null.
        /// </summary>
        public int? crashDuration { get; set; }

        public const string tableName = "crashs";

        public const string idString = "id";
        public const string timeString = "time";
        public const string crashDurationString = "crash_duration";

        protected override DataEntry ToDataEntry(string item)
        {
            int crashDuration;
            var crashData = new CrashData();
            crashData.id = int.Parse(GetDataValue(item, idString));
            crashData.time = DateTime.Parse(GetDataValue(item, timeString));
            if (int.TryParse(GetDataValue(item, crashDurationString), out crashDuration))
                crashData.crashDuration = crashDuration;
            return crashData;
        }

        public override string GetTableName()
        {
            return tableName;
        }
    }
}
