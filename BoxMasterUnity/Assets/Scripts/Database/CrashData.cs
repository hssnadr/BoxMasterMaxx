using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
        public const string name = "crash";

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

        internal override WWWForm GetForm()
        {
            var form = new WWWForm();
            form.AddField(idString, id.ToSQLFormat());
            form.AddField(timeString, time.ToSQLFormat());
            form.AddField(crashDurationString, crashDuration.ToSQLFormat());
            return form;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public override string GetTypeName()
        {
            return name;
        }

        public override string ToString()
        {
            return string.Format(culture, "CrashData = [id = {0}, time = {1}, crash_duration = {2}]", id, time, crashDuration);
        }

        public CrashData(int id, DateTime time, int? crashDuration)
        {
            this.id = id;
            this.time = time;
            this.crashDuration = crashDuration;
        }

        public CrashData() { }
    }
}
