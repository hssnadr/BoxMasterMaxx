using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public abstract class DataEntry
    {

        //protected abstract List<T> ToDataEntryList<T>(string dataEntryString) where T : DataEntry, new();

        protected static string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index + ":") + index.Length + 1);
            if (value.Contains("|"))
                value = value.Remove(value.IndexOf("|"));
            return value;
        }

        protected abstract DataEntry ToDataEntry(string item);

        public abstract string GetTableName();
        
        public static List<T> ToDataEntryList<T>(string dataEntryString) where T : DataEntry, new()
        {
            string[] items = dataEntryString.Split(';');
            var list = new List<T>();
            T newDataEntry = new T();
            foreach (var item in items)
            {
                var sessionData = (T)newDataEntry.ToDataEntry(item);
                list.Add(sessionData);
            }
            return list;
        }
    }
}
