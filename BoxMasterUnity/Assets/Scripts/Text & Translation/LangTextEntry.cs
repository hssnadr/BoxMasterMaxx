using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CRI.HitBox.Lang
{
    [System.Serializable]
    public struct LangTextEntry
    {
        [XmlAttribute("key")]
        public string key;
        [XmlText]
        public string text;

        public LangTextEntry(string key, string text)
        {
            this.key = key;
            this.text = text;
        }
    }
}
