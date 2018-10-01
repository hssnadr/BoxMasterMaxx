using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DataService : MonoBehaviour
    {
        public const string databaseName = "HitBox";

        IEnumerator LoadData<T>(System.Action<List<T>> callback) where T : DataEntry, new()
        {
            var dataEntry = new T();
            WWW www = new WWW(string.Format("http://localhost/{0}/SelectAll{1}Data.php", databaseName, ToCamelCase(dataEntry.GetTableName())));
            yield return www;
            string dataString = www.text;
            callback(DataEntry.ToDataEntryList<T>(dataString));
        }

        private string ToCamelCase(string s)
        {
            string res = s.Replace("_", " ");
            res = new CultureInfo("en-US").TextInfo.ToTitleCase(res);
            return res.Replace(" ", "");
        }

        private void Start()
        {
            StartCoroutine(LoadData<SessionData>((list) =>
            {
                foreach (var session in list)
                {
                    Debug.Log(session);
                }
            }));
            StartCoroutine(LoadData<InitData>((list) =>
            {
                foreach (var init in list)
                {
                    Debug.Log(init);
                }
            }));
            StartCoroutine(LoadData<PlayerData>((list) =>
            {
                foreach (var player in list)
                {
                    Debug.Log(player);
                }
            }));
        }
    }
}
