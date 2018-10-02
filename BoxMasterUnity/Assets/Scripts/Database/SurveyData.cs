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
        public int playerId { get; set; }
        /// <summary>
        /// An answer the player selected.
        /// </summary>
        public string answer { get; set; }

        public const string name = "player_survey";
        public const string tableName = "players_surveys";

        public const string playerIdString = "player_id";
        public const string answerString = "answer";

        protected override DataEntry ToDataEntry(string item)
        {
            var surveyData = new SurveyData();
            surveyData.playerId = int.Parse(GetDataValue(item, playerIdString));
            surveyData.answer = GetDataValue(item, answerString);
            return surveyData;
        }

        internal override WWWForm GetForm()
        {
            var form = new WWWForm();
            return form;
        }

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public override string ToString()
        {
            return string.Format(culture, "Survey = [player_id = {0}, answer = {1}]", playerId, answer);
        }
    }
}
