using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRI.HitBox.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string s)
        {
            string res = s.Replace("_", " ");
            res = new CultureInfo("en-US").TextInfo.ToTitleCase(res);
            return res.Replace(" ", "");
        }
    }
}
