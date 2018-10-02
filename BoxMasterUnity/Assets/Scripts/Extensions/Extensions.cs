using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRI.HitBox.Extensions
{
    public static class Extensions
    {
        public static string ToCamelCase(this string s)
        {
            string res = s.Replace("_", " ");
            res = new CultureInfo("en-US").TextInfo.ToTitleCase(res);
            return res.Replace(" ", "");
        }

        public static string ToSQLFormat(this object obj)
        {
            return obj.ToSQLFormat(CultureInfo.InvariantCulture);
        }

        public static string ToSQLFormat(this object obj, CultureInfo culture)
        {
            if (obj == null) return "NULL";
            if (obj.GetType() == typeof(string)) return "'" + obj.ToString() + "'";
            if (obj.GetType() == typeof(float)) return ((float)obj).ToString(culture);
            if (obj.GetType() == typeof(DateTime)) return "'" + ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            return obj.ToString();
        }
    }
}
