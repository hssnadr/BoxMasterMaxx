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
        public int timeOutScreen;
        /// <summary>
        /// Time until the application displays the home screen
        /// </summary>
        [XmlElement("timeout")]
        public int timeOut;
        /// <summary>
        /// Time until the menu goes back to its initial position
        /// </summary>
        [XmlElement("timeout_menu")]
        public int timeOutMenu;
        [XmlArray("page_settings")]
        [XmlArrayItem(typeof(ContentPageSettings), ElementName = "content_page")]
        [XmlArrayItem(typeof(TextOnlyPageSettings), ElementName = "text_page")]
        [XmlArrayItem(typeof(PlayerModeSettings), ElementName = "mode_page")]
        [XmlArrayItem(typeof(CatchScreenPageSettings), ElementName = "catchscreen_page")]
        [XmlArrayItem(typeof(SurveyPageSettings), ElementName = "survey_page")]
        [SerializeField]
        public ScreenSettings[] screenSettings;
        /// <summary>
        /// Path of the video of the catch screen.
        /// </summary>
        [XmlElement("catch_screen_video_path")]
        public string catchScreenVideoPath;
        /// <summary>
        /// Path of the video of the big screen.
        /// </summary>
        [XmlElement("big_screen_video_path")]
        public string bigScreenVideoPath;

        [XmlArray("menu_layout")]
        [XmlArrayItem(typeof(ButtonType), ElementName = "button_type")]
        public ButtonType[] menuLayout;
        /// <summary>
        /// The settings for the survey part of the application.
        /// </summary>
        [XmlElement("survey_settings")]
        public SurveySettings surveySettings;
        /// <summary>
        /// Volume of the audio.
        /// </summary>
        [XmlElement("audio_volume")]
        public float audioVolume;

        public MenuSettings(int timeOutScreen,
            int timeOut,
            int timeOutMenu,
            ScreenSettings[] screenSettings,
            string catchScreenVideoPath,
            string bigScreenVideoPath,
            ButtonType[] menuLayout,
            SurveySettings surveySettings,
            float audioVolume)
        {
            this.timeOutScreen = timeOutScreen;
            this.timeOut = timeOut;
            this.timeOutMenu = timeOutMenu;
            this.screenSettings = screenSettings;
            this.catchScreenVideoPath = catchScreenVideoPath;
            this.bigScreenVideoPath = bigScreenVideoPath;
            this.menuLayout = menuLayout;
            this.surveySettings = surveySettings;
            this.audioVolume = audioVolume;
        }
    }
}