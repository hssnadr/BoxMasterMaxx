using CRI.HitBox.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public abstract class DataEntry
    {
        public static readonly CultureInfo culture = CultureInfo.InvariantCulture;
        public static readonly NumberStyles numberStyles = System.Globalization.NumberStyles.Any;

        protected static string GetDataValue(string data, string index)
        {
            if (!data.Contains(index))
                return null;
            string value = data.Substring(data.IndexOf(index + ":") + index.Length + 1);
            if (value.Contains("|"))
                value = value.Remove(value.IndexOf("|"));
            return value;
        }


        public static List<T> ToDataEntryList<T>(string dataEntryString) where T : DataEntry, new()
        {
            string[] items = dataEntryString.Split(';');
            var list = new List<T>();
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var sessionData = ToDataEntry<T>(item);
                    list.Add(sessionData);
                }
            }
            return list;
        }

        protected static T ToDataEntry<T>(string item) where T : DataEntry, new()
        {
            var dataEntry = new T();
            IList<PropertyInfo> props = dataEntry.GetType().GetProperties().Where(x => x.IsDefined(typeof(FieldAttribute), false)).ToList();
            foreach (PropertyInfo prop in props)
            {
                string name = prop.GetCustomAttribute<FieldAttribute>().Name;
                string s = GetDataValue(item, name);
                object value;
                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                Type type = underlyingType == null ? prop.PropertyType : underlyingType;
                try
                {
                    if (!string.IsNullOrEmpty(s) || underlyingType == null)
                    {
                        if (type == typeof(bool))
                            value = (int)Convert.ChangeType(s, typeof(int), culture) != 0;
                        else if (type.IsEnum)
                            value = Enum.Parse(type, s);
                        else
                            value = Convert.ChangeType(s, type, culture);
                        prop.SetValue(dataEntry, value, null);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Field ({0}) Type ({1}) Value ({2}): {3}", name, type, s, e.Message));
                }
            }
            return dataEntry;
        }

        internal WWWForm GetForm()
        {
            var form = new WWWForm();
            IList<PropertyInfo> props = this.GetType().GetProperties().Where(x => x.IsDefined(typeof(FieldAttribute), false)).ToList();
            foreach (PropertyInfo prop in props)
            {
                form.AddField(prop.GetCustomAttribute<FieldAttribute>().Name, prop.GetValue(this).ToSQLFormat(culture));
            }
            return form;
        }

        public override string ToString()
        {
            string res = GetTypeName() + " = [";
            IList<PropertyInfo> props = this.GetType().GetProperties().Where(x => x.IsDefined(typeof(FieldAttribute), false)).ToList();
            for (int i = 0; i < props.Count; i++)
            {
                if (i != 0)
                    res += ", ";
                res += props[i].GetCustomAttribute<FieldAttribute>().Name + " = " + props[i].GetValue(this);
            }
            res += "]";
            return string.Format(culture, res);
        }

        public bool HasAutoIncrementPrimaryKey()
        {
            return GetType().GetProperties().Any(x => x.IsDefined(typeof(PrimaryKeyAttribute), false) && x.IsDefined(typeof(AutoIncrementAttribute), false));
        }

        public void SetAutoIncrementPrimaryKey(object value)
        {
            PropertyInfo prop = GetType().GetProperties().First(x => x.IsDefined(typeof(PrimaryKeyAttribute), false) && x.IsDefined(typeof(AutoIncrementAttribute), false));
            prop.SetValue(this, value);
        }

        public abstract string GetTableName();

        public abstract string GetTypeName();
    }
}
