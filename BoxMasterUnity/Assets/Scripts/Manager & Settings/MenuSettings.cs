// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace CRI.HitBox.Settings
{
    [Serializable]
    public struct MenuSettings
    {
        /// <summary>
        /// Time until the application displays the tap screen
        /// </summary>
        [XmlElement("timeout_screen")]
        public int timeoutScreen;

        /// <summary>
        /// Time until the application displays the home screen
        /// </summary>
        [XmlElement("timeout")]
        public int timeout;

        /// <summary>
        /// Time until the menu goes back to its initial position
        /// </summary>
        [XmlElement("menu_bar_timeout")]
        public int timeoutMenu;

        /// <summary>
        /// Volume of the audio.
        /// </summary>
        [XmlElement("audio_volume")]
        public float audioVolume;

        /// <summary>
        /// Layout of the menu bar.
        /// </summary>
        [XmlArray("menu_bar_layout")]
        [XmlArrayItem(typeof(ButtonType), ElementName = "button_type")]
        public ButtonType[] menuBarLayout;

        /// <summary>
        /// Array of ordered pages.
        /// </summary>
        [XmlArray("page_settings")]
        [XmlArrayItem(typeof(ContentPageSettings), ElementName = "content_page")]
        [XmlArrayItem(typeof(TextOnlyPageSettings), ElementName = "text_page")]
        [XmlArrayItem(typeof(PlayerModeSettings), ElementName = "mode_page")]
        [XmlArrayItem(typeof(CatchScreenSettings), ElementName = "catchscreen_page")]
        [XmlArrayItem(typeof(SurveyPageSettings), ElementName = "survey_page")]
        [SerializeField]
        public ScreenSettings[] pageSettings;

        /// <summary>
        /// Array of unordered screens.
        /// </summary>
        [XmlArray("screen_settings")]
        [XmlArrayItem(typeof(CountdownSettings), ElementName = "countdown")]
        [XmlArrayItem(typeof(CreditsSettings), ElementName = "credits")]
        [XmlArrayItem(typeof(BigScreenSettings), ElementName = "big_screen")]
        [XmlArrayItem(typeof(ScoreScreenSettings), ElementName = "score_screen")]
        [SerializeField]
        public ScreenSettings[] screenSettings;

        /// <summary>
        /// The settings for the survey part of the application.
        /// </summary>
        [XmlElement("survey_settings")]
        public SurveySettings surveySettings;

        public MenuSettings(int timeOutScreen,
            int timeOut,
            int timeOutMenu,
            ScreenSettings[] pageSettings,
            ScreenSettings[] screenSettings,
            string catchScreenVideoPath,
            string bigScreenVideoPath,
            ButtonType[] menuLayout,
            SurveySettings surveySettings,
            float audioVolume)
        {
            this.timeoutScreen = timeOutScreen;
            this.timeout = timeOut;
            this.timeoutMenu = timeOutMenu;
            this.pageSettings = pageSettings;
            this.screenSettings = screenSettings;
            this.menuBarLayout = menuLayout;
            this.surveySettings = surveySettings;
            this.audioVolume = audioVolume;
        }
    }
}