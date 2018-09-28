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
        public int id { get; set; }
        /// <summary>
        /// Time of the crash.
        /// </summary>
        public DateTime time { get; set; }
        /// <summary>
        /// Duration of the crash (in seconds).
        /// </summary>
        public int crashDuration { get; set; }
    }
}
