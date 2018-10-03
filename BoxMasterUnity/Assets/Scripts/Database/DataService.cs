using CRI.HitBox.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DataService : MonoBehaviour
    {
        /// <summary>
        /// The result of an insertion request.
        /// </summary>
        public struct InsertionResult
        {
            /// <summary>
            /// Was the insertion successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The id of the insert if the table had an auto_increment field.
            /// </summary>
            public int id;

            public InsertionResult(bool successful, int id)
            {
                this.successful = successful;
                this.id = id;
            }

            public override string ToString()
            {
                return string.Format("InsertionResult = [successful = {0}, id = {1}]", successful, id);
            }
        }
        public const string databaseName = "hitbox";        
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
                WWW www = await new WWW(string.Format("http://localhost/{0}/select_all_{1}.php", databaseName, dataEntry.GetTableName()));
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                string dataString = www.text;
                return DataEntry.ToDataEntryList<T>(dataString);
            }
            catch (Exception e)
            {
                Debug.LogError("Error for LoadData of type " + new T().GetType() + " :" + e.Message);
                return new List<T>();
            }
        }

        /// <summary>
        /// Loads a list of DataEntry from the database and applies an action to the list when the list finished loading.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <param name="callback">The action that will be called on the list when it finishes loading.</param>
        public async void LoadData<T>(Action<List<T>> callback) where T : DataEntry, new()
        {
            var list = await LoadData<T>();
            callback(list);
        }

        /// <summary>
        /// Inserts a DataEntry into a database.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <returns>An instance of InsertionResult.</returns>
        public async Task<InsertionResult> InsertData(DataEntry dataEntry)
        {
            try
            {
                WWW www = await new WWW(string.Format("http://localhost/{0}/insert_{1}.php", databaseName, dataEntry.GetTypeName()), dataEntry.GetForm());
                int id = 0;
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                    int.TryParse(www.text, out id);
                }
                return new InsertionResult(true, id); 
            }
            catch (Exception e)
            {
                Debug.LogError("Error during insertion of data entry " + dataEntry + ": " + e.Message);
                return new InsertionResult(false, 0);
            }
        }

        /// <summary>
        /// Inserts a DataEntry into a database and applies an action to the resulting InsertionResult.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <param name="callback">The action that will be called when the insertion ends.</param>
        public async void InsertData(DataEntry dataEntry, Action<InsertionResult> callback)
        {
            var result = await InsertData(dataEntry);
            callback(result);
        }

        async void Start()
        {
            InsertData(new CrashData(0, DateTime.Now, null), result => Debug.Log(result));
            InsertData(new CrashData(0, DateTime.Now, UnityEngine.Random.Range(0, 100000)), result => Debug.Log(result));
            InsertData(new HitData(0, new PlayerData() { id = 1 }, DateTime.Now, UnityEngine.Random.insideUnitCircle * 10.0f, UnityEngine.Random.Range(0, 2) == 1, UnityEngine.Random.insideUnitSphere * 10.0f, UnityEngine.Random.insideUnitSphere * 10.0f), result => Debug.Log(result));
            InsertData(InitData.CreateFromApplicationSettings(ApplicationSettings.Load(Path.Combine(Application.streamingAssetsPath, ApplicationManager.appSettingsPath))), result => Debug.Log(result));
            var sessionDataTask = LoadData<SessionData>();
            var initDataTask = LoadData<InitData>();
            var playerDataTask = LoadData<PlayerData>();
            var surveyDataTask = LoadData<SurveyData>();
            var targetCountThresholdDataTask = LoadData<TargetCountThresholdData>();
            var hitDataTask = LoadData<HitData>();
            var crashDataTask = LoadData<CrashData>();
            await Task.WhenAll(sessionDataTask, initDataTask, playerDataTask, surveyDataTask, targetCountThresholdDataTask, hitDataTask, crashDataTask);
            var sessionData = await sessionDataTask;
            var initData = await initDataTask;
            var playerData = await playerDataTask;
            var surveyData = await surveyDataTask;
            var targetCountThresholdData = await targetCountThresholdDataTask;
            var hitData = await hitDataTask;
            var crashData = await crashDataTask;
            sessionData.ForEach(x => Debug.Log(x));
            initData.ForEach(x => Debug.Log(x));
            playerData.ForEach(x => Debug.Log(x));
            surveyData.ForEach(x => Debug.Log(x));
            targetCountThresholdData.ForEach(x => Debug.Log(x));
            hitData.ForEach(x => Debug.Log(x));
            crashData.ForEach(x => Debug.Log(x));
        }
    }
}
