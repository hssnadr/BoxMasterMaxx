using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class SurveyData
    {
        /// <summary>
        /// The index of the player.
        /// </summary>
        [PrimaryKey]
        public int playerId { get; set; }
        /// <summary>
        /// An answer the player selected.
        /// </summary>
        [Indexed]
        public string answer { get; set; }
    }
}
