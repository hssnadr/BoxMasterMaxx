// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Xml;
using System.Text;
using System.IO;
using CRI.HitBox.Lang;


namespace CRI.HitBox.Settings
{
    /// <summary>
    /// A button type for the menu bar.
    /// </summary>
    public enum ButtonType
    {
        [XmlEnum("start")]
        Start,
        [XmlEnum("sound")]
        Sound,
        [XmlEnum("copyright")]
        Copyright,
        [XmlEnum("separator")]
        Separator,
    }

    /// <summary>
    /// The settings of the game, which are the different timeout values, the settings for the serial components, the settings for the gameplay and the settings for the differents pages of the interface.
    /// </summary>
    [System.Serializable]
    public class GameSettings
    {
        /// <summary>
        /// The name of the application.
        /// </summary>
        public string name;
        /// <summary>
        /// All of the languages available for the translation of the application.
        /// </summary>
        [XmlArray("lang_app_available")]
        [XmlArrayItem(typeof(LangApp), ElementName = "lang_app")]
        [SerializeField]
        public LangApp[] langAppAvailable;

        /// <summary>
        /// Code of the enabled languages in priority order of appearance in the menus
        /// </summary>
        [XmlArray("lang_app_enable")]
        [XmlArrayItem(typeof(string), ElementName = "code")]
        [SerializeField]
        public string[] langAppEnableCodes;

        /// <summary>
        /// Enabled languages in priority order of appearance in the menus
        /// </summary>
        protected LangApp[] _langAppEnable;

        /// <summary>
        /// Enabled languages in priority order of appearance in the menus.
        /// </summary>
        [XmlIgnore]
        public IList<LangApp> langAppEnable
        {
            get
            {
                return _langAppEnable.ToList().AsReadOnly();
            }
        }
        
        /// <summary>
        /// Serialized version of the cursor visible field.
        /// </summary>
        [XmlElement("cursor_visible")]
        public string cursorVisibleSerialized
        {
            get { return this.cursorVisible ? "True" : "False"; }
            set
            {
                if (value.ToUpper().Equals("TRUE"))
                    this.cursorVisible = true;
                else if (value.ToUpper().Equals("FALSE"))
                    this.cursorVisible = false;
                else
                    this.cursorVisible = XmlConvert.ToBoolean(value);
            }
        }

        /// <summary>
        /// Is the cursor enabled ?
        /// </summary>
        [XmlIgnore]
        public bool cursorVisible { get; private set; }

        /// <summary>
        /// Color of the P1 as a hex.
        /// </summary>
        [XmlElement("p1_color")]
        public string p1Color;

        /// <summary>
        /// Color of the P2 as a hex.
        /// </summary>
        [XmlElement("p2_color")]
        public string p2Color;

        /// <summary>
        /// The settings for all the menu elements.
        /// </summary>
        [XmlElement("menu_settings")]
        public MenuSettings menuSettings;

        /// <summary>
        /// The settings for the gameplay components.
        /// </summary>
        [XmlElement("gameplay_settings")]
        public GameplaySettings gameplaySettings;

        /// <summary>
        /// The settings for the serial components.
        /// </summary>
        [XmlElement("serial_settings")]
        public SerialSettings serialSettings;

        /// <summary>
        /// Gets the default language, which is the first of the list.
        /// </summary>
        /// <value>The default language.</value>
        [XmlIgnore]
        public LangApp defaultLanguage
        {
            get
            {
                return _langAppEnable[0];
            }
        }

        public const int PlayerNumber = 2;


        /// <summary>
        /// Save the data to an XML file.
        /// </summary>
        /// <param name="path">The path where the XML file will be saved.</param>
        public void Save(string path)
        {
            var serializer = new XmlSerializer(typeof(GameSettings));
            using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Create))
            using (var xmlWriter = new XmlTextWriter(stream, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;
                serializer.Serialize(xmlWriter, this);
            }
        }

        /// <summary>
        /// Loads a game setting data from the XML file at the specified path.
        /// </summary>
        /// <returns>The game setting data.</returns>
        /// <param name="path">The path here the XML file is located.</param>
        public static GameSettings Load(string path)
        {
            var serializer = new XmlSerializer(typeof(GameSettings));
            using (var stream = new FileStream(Path.Combine(Application.dataPath, path), FileMode.Open))
            {
                var gameSettings = serializer.Deserialize(stream) as GameSettings;
                gameSettings._langAppEnable = new LangApp[gameSettings.langAppEnableCodes.Length];
                for (int i = 0; i < gameSettings.langAppEnableCodes.Length; i++)
                {
                    var code = gameSettings.langAppEnableCodes[i];
                    var langApp = gameSettings.langAppAvailable.First(x => x.code == code);
                    gameSettings._langAppEnable[i] = langApp;
                }
                return gameSettings;
            }
        }

        /// <summary>
        /// Loads a game setting from an XML text.
        /// </summary>
        /// <returns>The game setting data.</returns>
        /// <param name="text">A text in XML format that contains the data that will be loaded.</param>
        public static GameSettings LoadFromText(string text)
        {
            var serializer = new XmlSerializer(typeof(GameSettings));
            return serializer.Deserialize(new StringReader(text)) as GameSettings;
        }
    }
}
