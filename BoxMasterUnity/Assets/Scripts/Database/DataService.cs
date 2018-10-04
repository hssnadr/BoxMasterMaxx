using CRI.HitBox.Extensions;
using CRI.HitBox.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DataService : MonoBehaviour
    {
        /// <summary>
        /// The result of an insertion request.
        /// </summary>
        public struct InsertionResult<T> where T: DataEntry
        {
            /// <summary>
            /// Was the insertion successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The id of the insert if the table had an auto_increment field.
            /// </summary>
            public int id;
            /// <summary>
            /// The inserted data.
            /// </summary>
            public T data;

            public InsertionResult(bool successful, int id, T data)
            {
                this.successful = successful;
                this.id = id;
                this.data = data;
            }

            public override string ToString()
            {
                return string.Format("InsertionResult = [successful = {0}, id = {1}, data = {2}]", successful, id, data);
            }
        }

        /// <summary>
        /// The result of an update request.
        /// </summary>
        public struct UpdateResult<T> where T : DataEntry
        {
            /// <summary>
            /// Was the update successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The update data.
            /// </summary>
            public T data;

            public UpdateResult(bool successful, T data)
            {
                this.successful = successful;
                this.data = data;
            }

            public override string ToString()
            {
                return string.Format("UpdateResult = [successful = {0}, data = {1}]", successful, data);
            }
        }

        /// <summary>
        /// The result of an selection request.
        /// </summary>
        public struct SelectResult<T> where T : DataEntry
        {
            /// <summary>
            /// Was the selection successful ?
            /// </summary>
            public bool successful;
            /// <summary>
            /// The selected dataList.
            /// </summary>
            public List<T> dataList;
            /// <summary>
            /// Number of lines selected.
            /// </summary>
            public int count
            {
                get
                {
                    return dataList.Count;
                }
            }

            public SelectResult(bool successful, List<T> dataList)
            {
                this.successful = successful;
                this.dataList = dataList;
            }

            public override string ToString()
            {
                return string.Format("SelectResult = [successful = {0}, data = {1}, count = {2}]", successful, dataList, count);
            }
        }

        public const string databaseName = "hitbox";        
        /// <summary>
        /// Loads a list of DataEntry from the database.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <returns>A list of DataEntry</returns>
        public async Task<SelectResult<T>> LoadData<T>() where T : DataEntry, new()
        {
            try
            {
                var dataEntry = new T();
                WWW www = await new WWW(string.Format("http://localhost/{0}/select_all_{1}.php", databaseName, dataEntry.GetTableName()));
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                }
                string dataString = www.text;
                return new SelectResult<T>(true, DataEntry.ToDataEntryList<T>(dataString));
            }
            catch (Exception e)
            {
                Debug.LogError("Error for LoadData of type " + new T().GetType() + " :" + e.Message);
                return new SelectResult<T>(false, null);
            }
        }

        /// <summary>
        /// Loads a list of DataEntry from the database and applies an action to the list when the list finished loading.
        /// </summary>
        /// <typeparam name="T">A non abstract type of DataEntry</typeparam>
        /// <param name="callback">The action that will be called on the list when it finishes loading.</param>
        public async void LoadData<T>(Action<SelectResult<T>> callback) where T : DataEntry, new()
        {
            var result = await LoadData<T>();
            callback(result);
        }

        /// <summary>
        /// Inserts a DataEntry into a database.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <returns>An instance of InsertionResult.</returns>
        public async Task<InsertionResult<T>> InsertData<T>(T dataEntry) where T : DataEntry
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
                if (dataEntry.HasAutoIncrementPrimaryKey())
                    dataEntry.SetAutoIncrementPrimaryKey(id);
                return new InsertionResult<T>(true, id, dataEntry); 
            }
            catch (Exception e)
            {
                Debug.LogError("Error during insertion of data entry " + dataEntry + ": " + e.Message);
                return new InsertionResult<T>(false, 0, dataEntry);
            }
        }

        /// <summary>
        /// Inserts a DataEntry into a database and applies an action to the resulting InsertionResult.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <param name="callback">The action that will be called when the insertion ends.</param>
        public async void InsertData<T>(T dataEntry, Action<InsertionResult<T>> callback) where T : DataEntry
        {
            var result = await InsertData(dataEntry);
            callback(result);
        }

        /// <summary>
        /// Update a DataEntry of a database.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <returns>An instance of UpdateResult</returns>
        public async Task<UpdateResult<T>> UpdateData<T>(T dataEntry) where T : DataEntry
        {
            try
            {
                WWW www = await new WWW(string.Format("http://localhost/{0}/update_{1}.php", databaseName, dataEntry.GetTypeName()), dataEntry.GetForm());
                if (!string.IsNullOrEmpty(www.error))
                {
                    throw new Exception(www.error);
                }
                if (!string.IsNullOrEmpty(www.text))
                {
                    if (www.text.ToUpper().Contains("ERROR"))
                        throw new Exception(www.text);
                }
                return new UpdateResult<T>(true, dataEntry);
            }
            catch (Exception e)
            {
                Debug.LogError("Error during update of data entry " + dataEntry + ": " + e.Message);
                return new UpdateResult<T>(false, dataEntry);
            }
        }

        /// <summary>
        /// Update a DataEntry of a database and applies an action to the resulting UpdateResult.
        /// </summary>
        /// <param name="dataEntry">A data entry.</param>
        /// <param name="callback">The action that will be called when the update ends.</param>
        public async void UpdateData<T>(T dataEntry, Action<UpdateResult<T>> callback) where T : DataEntry
        {
            var result = await UpdateData(dataEntry);
            callback(result);
        }

        private void InsertTest()
        {
            var applicationSettings = ApplicationSettings.Load(Path.Combine(Application.streamingAssetsPath, ApplicationManager.appSettingsPath));
            InsertData(new CrashData(0, DateTime.Now, null), result => Debug.Log(result));
            InsertData(new CrashData(0, DateTime.Now, UnityEngine.Random.Range(0, 100000)), result => Debug.Log(result));
            InsertData(InitData.CreateFromApplicationSettings(applicationSettings), result =>
            {
                Debug.Log(result);
                if (result.successful)
                {
                    InsertData(new SessionData(0, result.data, DateTime.Now, "fr", UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(0, 1000), UnityEngine.Random.Range(0, 2000),
                        UnityEngine.Random.Range(0, 2) == 1, UnityEngine.Random.Range(0, 2) == 1, (GameMode)UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2000),
                        UnityEngine.Random.Range(result.data.minPrecisionRating, result.data.maxPrecisionRating), UnityEngine.Random.Range(result.data.minSpeedRating, result.data.maxSpeedRating),
                        UnityEngine.Random.Range(result.data.comboMin, result.data.comboMax)), result2 =>
                        {
                            Debug.Log(result2);
                            if (result2.successful)
                                InsertData(new PlayerData(0, result2.data, UnityEngine.Random.Range(0, 2), UnityEngine.Random.insideUnitSphere * 10.0f), result3 =>
                                {
                                    Debug.Log(result3);
                                    if (result3.successful)
                                    {
                                        InsertData(new HitData(0, result3.data, DateTime.Now, UnityEngine.Random.insideUnitCircle * 10.0f, UnityEngine.Random.Range(0, 2) == 1, UnityEngine.Random.insideUnitSphere * 10.0f, UnityEngine.Random.insideUnitSphere * 10.0f), result4 => Debug.Log(result4));
                                        InsertData(new SurveyData(result3.data, "Q1A3"), result4 => Debug.Log(result4));
                                    }
                                });
                        });
                    for (int i = 0; i < applicationSettings.gameSettings.targetCountThreshold.Length; i++)
                    {
                        InsertData(new TargetCountThresholdData(i, result.data, applicationSettings.gameSettings.targetCountThreshold[i]), x => Debug.Log(x));
                    }
                }
            }
            );
        }

        private void PrintResult<T>(T obj)
        {
            Debug.Log(obj);
        }

        async Task SelectTest()
        {
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
            sessionData.dataList.ForEach(x => Debug.Log(x));
            initData.dataList.ForEach(x => Debug.Log(x));
            playerData.dataList.ForEach(x => Debug.Log(x));
            surveyData.dataList.ForEach(x => Debug.Log(x));
            targetCountThresholdData.dataList.ForEach(x => Debug.Log(x));
            hitData.dataList.ForEach(x => Debug.Log(x));
            crashData.dataList.ForEach(x => Debug.Log(x));
        }

        void UpdateTest()
        {
            LoadData<CrashData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last(x => x.crashDuration != null);
                    data.crashDuration += 10;
                    UpdateData(data, updateResult => Debug.Log(updateResult));
                }
            });
            LoadData<PlayerData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.playerIndex += 1;
                    data.setupHitPosition *= 2.0f;
                    UpdateData(data, updateResult => Debug.Log(updateResult));
                }
            });
            LoadData<HitData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.position *= 2.0f;
                    data.successful ^= true;
                    data.targetCenter *= 2.0f;
                    data.targetSpeedVector *= 2.0f;
                    data.time = DateTime.Now;
                    UpdateData(data, PrintResult);
                }
            });
            LoadData<SessionData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.debugExit ^= true;
                    data.gameMode = data.gameMode == GameMode.P1 ? GameMode.P2 : GameMode.P1;
                    data.highestComboMultiplier += 1;
                    data.langCode = "en";
                    data.precisionRating *= 2;
                    data.score += 10;
                    data.speedRating *= 2;
                    data.time = DateTime.Now;
                    data.timeout ^= true;
                    data.timeoutScreenCount += 1;
                    data.timeSpentOnMenu += 1;
                    data.timeSpentTotal += 1;
                    UpdateData(data, PrintResult);
                }
            });
            LoadData<TargetCountThresholdData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    foreach (var ct in selectResult.dataList)
                    {
                        ct.countThreshold++;
                        UpdateData(ct, PrintResult);
                    }
                }
            });
            LoadData<InitData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.comboDuration += 1;
                    data.comboMin += 1;
                    data.comboMax += 1;
                    data.comboIncrement += 1;
                    data.comboDurationMultiplier += 1;
                    data.delayOffHit += 1;
                    data.gameDuration += 1;
                    data.hitMaxPoints += 1;
                    data.hitMinPoints += 1;
                    data.hitTolerance += 1;
                    data.impactThreshold += 1;
                    data.maxPrecisionRating += 1;
                    data.maxSpeedRating += 1;
                    data.minPrecisionRating += 1;
                    data.minSpeedRating += 1;
                    data.targetCooldown += 1;
                    data.targetHorizontalMovementSpeed += 1;
                    data.targetMaxAngularVelocity += 1;
                    data.targetP1ActivationDelay += 1;
                    data.targetRotationSpeed += 1;
                    data.targetZRotationSpeed += 1;
                    data.timeout += 1;
                    data.timeoutScreen += 1;
                    UpdateData(data, updateResult =>
                    {
                        Debug.Log(updateResult);
                    });
                }
            });
        }

        async void Start()
        {
            InsertTest();
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                UpdateTest();
        }
    }
}
