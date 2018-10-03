using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class SurveyData : DataEntry
    {
        /// <summary>
        /// The index of the player.
        /// </summary>
        [Field("player_id"), PrimaryKey]
        public int playerId { get; set; }
        /// <summary>
        /// An answer the player selected.
        /// </summary>
        [Field("answer"), PrimaryKey]
        public string answer { get; set; }

        public const string name = "player_survey";
        public const string tableName = "players_surveys";

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public SurveyData(PlayerData player, string answer)
        {
            playerId = player.id;
            this.answer = answer;
        }

        public SurveyData() { }
    }
}
