using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public abstract class DataEntry
    {

        //protected abstract List<T> ToDataEntryList<T>(string dataEntryString) where T : DataEntry, new();

        protected static string GetDataValue(string data, string index)
        {
            string value = data.Substring(data.IndexOf(index + ":") + index.Length + 1);
            value = value.Remove(value.IndexOf("|"));
            return value;
        }
    }
}
