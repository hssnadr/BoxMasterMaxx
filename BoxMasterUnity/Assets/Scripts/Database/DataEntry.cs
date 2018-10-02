using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public abstract class DataEntry
    {
        public static readonly CultureInfo culture = CultureInfo.InvariantCulture;
        public static readonly NumberStyles numberStyles = System.Globalization.NumberStyles.Any;

        protected string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index + ":") + index.Length + 1);
            if (value.Contains("|"))
                value = value.Remove(value.IndexOf("|"));
            return value;
        }

        protected abstract DataEntry ToDataEntry(string item);

        internal abstract WWWForm GetForm();

        public abstract string GetTableName();

        public abstract string GetTypeName();

        public static List<T> ToDataEntryList<T>(string dataEntryString) where T : DataEntry, new()
        {
            string[] items = dataEntryString.Split(';');
            var list = new List<T>();
            T newDataEntry = new T();
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var sessionData = (T)newDataEntry.ToDataEntry(item);
                    list.Add(sessionData);
                }
            }
            return list;
        }
    }
}
