using CRI.HitBox.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DataService : MonoBehaviour
    {
        public const string databaseName = "HitBox";
        
        /// <summary>
        /// Loads a list of DataEntry from the database.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <returns>A list of DataEntry</returns>
        public async Task<List<T>> LoadData<T>() where T : DataEntry, new()
        {
            try
            {
                var dataEntry = new T();
                WWW www = await new WWW(string.Format("http://localhost/{0}/SelectAll{1}Data.php", databaseName, dataEntry.GetTableName().ToCamelCase()));
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception();
                }
                string dataString = www.text;
                return DataEntry.ToDataEntryList<T>(dataString);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return new List<T>();
            }
        }

        /// <summary>
        /// Loads a list of DataEntry from the database and applies an action to the list when the list finished loading.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <param name="callback">The action that will be called on the list when it finishes loading.</param>
        public async void LoadDataAction<T>(Action<List<T>> callback) where T : DataEntry, new()
        {
            var list = await LoadData<T>();
            callback(list);
        }

        async void Start()
        {
            var sessionDataTask = LoadData<SessionData>();
            var initDataTask = LoadData<InitData>();
            var playerDataTask = LoadData<PlayerData>();
            var surveyDataTask = LoadData<SurveyData>();
            var targetCountThresholdDataTask = LoadData<TargetCountThresholdData>();
            var hitDataTask = LoadData<HitData>();
            await Task.WhenAll(sessionDataTask, initDataTask, playerDataTask, surveyDataTask, targetCountThresholdDataTask, hitDataTask);
            var sessionData = await sessionDataTask;
            var initData = await initDataTask;
            var playerData = await playerDataTask;
            var surveyData = await surveyDataTask;
            var targetCountThresholdData = await targetCountThresholdDataTask;
            var hitData = await hitDataTask;
            sessionData.ForEach(x => Debug.Log(x));
            initData.ForEach(x => Debug.Log(x));
            playerData.ForEach(x => Debug.Log(x));
            surveyData.ForEach(x => Debug.Log(x));
            targetCountThresholdData.ForEach(x => Debug.Log(x));
            hitData.ForEach(x => Debug.Log(x));
        }
    }
}
