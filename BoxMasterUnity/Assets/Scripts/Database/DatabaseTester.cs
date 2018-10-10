using CRI.HitBox.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
#if UNITY_EDITOR
    public class DatabaseTester : MonoBehaviour
    {
        private void InsertTest()
        {
            var applicationSettings = ApplicationSettings.Load(Path.Combine(Application.streamingAssetsPath, ApplicationManager.appSettingsPath));
            DataService.InsertData(new CrashData(0, DateTime.Now, null), result => Debug.Log(result));
            DataService.InsertData(new CrashData(0, DateTime.Now, UnityEngine.Random.Range(0, 100000)), result => Debug.Log(result));
            DataService.InsertData(InitData.CreateFromApplicationSettings(applicationSettings), result =>
            {
                Debug.Log(result);
                if (result.successful)
                {
                    DataService.InsertData(new SessionData(0, result.data, DateTime.Now, "fr", UnityEngine.Random.Range(0, 100), UnityEngine.Random.Range(0, 1000), UnityEngine.Random.Range(0, 2000),
                        UnityEngine.Random.Range(0, 2) == 1, UnityEngine.Random.Range(0, 2) == 1, (GameMode)UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2000),
                        UnityEngine.Random.Range(result.data.minPrecisionRating, result.data.maxPrecisionRating), UnityEngine.Random.Range(result.data.minSpeedRating, result.data.maxSpeedRating),
                        UnityEngine.Random.Range(result.data.comboMin, result.data.comboMax)), result2 =>
                        {
                            Debug.Log(result2);
                            if (result2.successful)
                                DataService.InsertData(new PlayerData(0, result2.data, UnityEngine.Random.Range(0, 2), UnityEngine.Random.insideUnitSphere * 10.0f), result3 =>
                                {
                                    Debug.Log(result3);
                                    if (result3.successful)
                                    {
                                        DataService.InsertData(new HitData(0, result3.data, DateTime.Now, UnityEngine.Random.insideUnitCircle * 10.0f, UnityEngine.Random.Range(0, 2) == 1, UnityEngine.Random.insideUnitSphere * 10.0f, UnityEngine.Random.insideUnitSphere * 10.0f), result4 => Debug.Log(result4));
                                        DataService.InsertData(new SurveyData(result3.data, "Q1A3"), result4 => Debug.Log(result4));
                                    }
                                });
                        });
                    for (int i = 0; i < applicationSettings.gameSettings.targetCountThreshold.Length; i++)
                    {
                        DataService.InsertData(new TargetCountThresholdData(i, result.data, applicationSettings.gameSettings.targetCountThreshold[i]), x => Debug.Log(x));
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
            var sessionDataTask = DataService.LoadData<SessionData>();
            var initDataTask = DataService.LoadData<InitData>();
            var playerDataTask = DataService.LoadData<PlayerData>();
            var surveyDataTask = DataService.LoadData<SurveyData>();
            var targetCountThresholdDataTask = DataService.LoadData<TargetCountThresholdData>();
            var hitDataTask = DataService.LoadData<HitData>();
            var crashDataTask = DataService.LoadData<CrashData>();
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
            DataService.LoadData<CrashData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last(x => x.crashDuration != null);
                    data.crashDuration += 10;
                    DataService.UpdateData(data, updateResult => Debug.Log(updateResult));
                }
            });
            DataService.LoadData<PlayerData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.playerIndex += 1;
                    data.setupHitPosition *= 2.0f;
                    DataService.UpdateData(data, updateResult => Debug.Log(updateResult));
                }
            });
            DataService.LoadData<HitData>(selectResult =>
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
                    DataService.UpdateData(data, PrintResult);
                }
            });
            DataService.LoadData<SessionData>(selectResult =>
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
                    DataService.UpdateData(data, PrintResult);
                }
            });
            DataService.LoadData<TargetCountThresholdData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    foreach (var ct in selectResult.dataList)
                    {
                        ct.countThreshold++;
                        DataService.UpdateData(ct, PrintResult);
                    }
                }
            });
            DataService.LoadData<SurveyData>(selectResult =>
            {
                Debug.Log(selectResult);
                if (selectResult.successful)
                {
                    var data = selectResult.dataList.Last();
                    data.answer += "test";
                    DataService.UpdateData(data, PrintResult);
                }
            });
            DataService.LoadData<InitData>(selectResult =>
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
                    DataService.UpdateData(data, updateResult =>
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
#endif
}

