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
        [Field("id"), AutoIncrement, PrimaryKey]
        public int id { get; set; }
        /// <summary>
        /// Time of the crash.
        /// </summary>
        [Field("time")]
        public DateTime time { get; set; }
        /// <summary>
        /// Duration of the crash (in seconds). Can be null.
        /// </summary>
        [Field("crash_duration")]
        public int? crashDuration { get; set; }

        public const string tableName = "crashs";
        public const string name = "crash";

        public override string GetTableName()
        {
            return tableName;
        }

        public override string GetTypeName()
        {
            return name;
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
