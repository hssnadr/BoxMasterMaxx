using SQLite4Unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DataService
    {

        private SQLiteConnection _connection;

        public DataService(string dbName)
        {
            var dbPath = Path.Combine(Application.streamingAssetsPath, dbName);

            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        }

        private void DropAllTables()
        {
            _connection.DropTable<CrashData>();
            _connection.DropTable<HitData>();
            _connection.DropTable<InitData>();
            _connection.DropTable<PlayerData>();
            _connection.DropTable<SessionData>();
            _connection.DropTable<SurveyData>();
            _connection.DropTable<TargetCountThresholdData>();
        }

        private void CreateTables()
        {
            _connection.CreateTable<CrashData>();
            _connection.CreateTable<HitData>();
            _connection.CreateTable<InitData>();
            _connection.CreateTable<PlayerData>();
            _connection.CreateTable<SessionData>();
            _connection.CreateTable<SurveyData>();
            _connection.CreateTable<TargetCountThresholdData>();
        }

        public void CreateDatabase()
        {
            DropAllTables();
            CreateTables();
        }

        public int InsertSession(SessionData session)
        {
            return _connection.Insert(session);
        }

        public int UpdateSession(SessionData session)
        {
            return _connection.Update(session);
        }

        public int InsertCrash(CrashData crashData)
        {
            return _connection.Insert(crashData);
        }

        public int UpdateCrash(CrashData crashData)
        {
            return _connection.Update(crashData);
        }

        public int InsertHit(CrashData hitData)
        {
            return _connection.Insert(hitData);
        }

        public int UpdateHit(HitData hitData)
        {
            return _connection.Update(hitData);
        }

        public int InsertInit(InitData initData)
        {
            return _connection.Insert(initData);
        }

        public int UpdateInit(InitData initData)
        {
            return _connection.Update(initData);
        }

        public int InsertPlayer(PlayerData playerData)
        {
            return _connection.Insert(playerData);
        }

        public int UpdatePlayer(PlayerData playerData)
        {
            return _connection.Update(playerData);
        }

        public int InsertSurvey(SurveyData survey)
        {
            return _connection.Insert(survey);
        }

        public int UpdateSurvey(SurveyData survey)
        {
            return _connection.Update(survey);
        }

        public int InsertTargetCountThreshold(TargetCountThresholdData targetCountThresholdData)
        {
            return _connection.Insert(targetCountThresholdData);
        }

        public int UpdateTargetCountThreshold(TargetCountThresholdData targetCountThresholdData)
        {
            return _connection.Update(targetCountThresholdData);
        }
    }
}
