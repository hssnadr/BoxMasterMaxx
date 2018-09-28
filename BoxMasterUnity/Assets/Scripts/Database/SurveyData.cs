using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public const string playerIdString = "player_id";
        public const string answerString = "answer";

        protected static SurveyData ToSurveyData(string item)
        {
            var surveyData = new SurveyData();
            surveyData.playerId = int.Parse(GetDataValue(item, playerIdString));
            surveyData.answer = GetDataValue(item, answerString);
            return surveyData;
        }
    }
}
