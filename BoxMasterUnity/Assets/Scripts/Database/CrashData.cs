using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class CrashData
    {
        /// <summary>
        /// Index of the crash data.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// Time of the crash.
        /// </summary>
        [NotNull]
        public DateTime time { get; set; }
        /// <summary>
        /// Duration of the crash.
        /// </summary>
        [NotNull]
        public float crashDuration { get; set; }
    }
}
