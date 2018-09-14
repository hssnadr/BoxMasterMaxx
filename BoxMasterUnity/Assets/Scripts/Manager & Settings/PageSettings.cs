// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.Linq;
using CRI.HitBox.Lang;

namespace CRI.HitBox.Settings
{
    [Serializable]
    public class CountdownSettings : ScreenSettings, IAudioContainer
    {
        /// <summary>
        /// The path of the audio file.
        /// </summary>
        [XmlElement("audio")]
        public StringCommon audioPath;

        public StringCommon GetAudioPath()
        {
            return audioPath;
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.Countdown;
        }
    }
    [Serializable]
    public class CreditsSettings : ScreenSettings, IImageContainer
    {
        [Serializable]
        public struct CreditsContent
        {
            /// <summary>
            /// Main text of the content.
            /// </summary>
            [XmlElement("text")]
            public StringCommon text;
            /// <summary>
            /// Copyright section of the content.
            /// </summary>
            [XmlElement("copyright")]
            public StringCommon copyright;
            /// <summary>
            /// Array of logos for the content.
            /// </summary>
            [XmlArray("logos")]
            [XmlArrayItem(typeof(StringCommon), ElementName = "logo")]
            public StringCommon[] logos;
        }

        /// <summary>
        /// The text key of the title.
        /// </summary>
        [XmlElement("title")]
        public StringCommon title;
        /// <summary>
        /// Content of the left side of the credits screen.
        /// </summary>
        [XmlElement("left_content")]
        public CreditsContent leftContent;
        /// <summary>
        /// Content of the right side of the credits screen.
        /// </summary>
        [XmlElement("right_content")]
        public CreditsContent rightContent;
        /// <summary>
        /// Text of the margin.
        /// </summary>
        [XmlElement("margin_text")]
        public StringCommon marginText;

        public StringCommon[] GetImagePaths()
        {
            if (leftContent.logos == null && rightContent.logos != null)
                return rightContent.logos;
            if (leftContent.logos != null && rightContent.logos == null)
                return leftContent.logos;
            if (rightContent.logos != null && rightContent.logos != null)
                leftContent.logos.Concat(rightContent.logos).ToArray();
            return new StringCommon[0];
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.Credits;
        }
    }
    /// <summary>
    /// The settings for the big screen.
    /// </summary>
    [Serializable]
    public class BigScreenSettings : ScreenSettings, IVideoContainer
    {
        /// <summary>
        /// The path of the video file.
        /// </summary>
        [XmlElement("video")]
        public StringCommon videoPath;

        public StringCommon GetVideoPath()
        {
            return videoPath;
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.BigScreen;
        }
    }

    /// <summary>
    /// The settings for the score screen.
    /// </summary>
    [Serializable]
    public class ScoreScreenSettings : ScreenSettings, IAudioContainer
    {
        /// <summary>
        /// The path of the audio file.
        /// </summary>
        [XmlElement("audio")]
        public StringCommon audioPath;
        /// <summary>
        /// Text of the score.
        /// </summary>
        [XmlElement("score_text")]
        public StringCommon scoreText;
        /// <summary>
        /// Text of the best score.
        /// </summary>
        [XmlElement("best_score_text")]
        public StringCommon bestScoreText;
        /// <summary>
        /// Text of the precision.
        /// </summary>
        [XmlElement("precision_text")]
        public StringCommon precisionText;
        /// <summary>
        /// Text of the speed.
        /// </summary>
        [XmlElement("speed_text")]
        public StringCommon speedText;
        /// <summary>
        /// Text of the points indicator.
        /// </summary>
        [XmlElement("points_text")]
        public StringCommon pointsText;
        /// <summary>
        /// Text of the title.
        /// </summary>
        [XmlElement("title")]
        public StringCommon title;
        /// <summary>
        /// Text of the thanks.
        /// </summary>
        [XmlElement("thanks_text")]
        public StringCommon thanksText;

        public StringCommon GetAudioPath()
        {
            return audioPath;
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.ScoreScreen;
        }
    }

    /// <summary>
    /// The settings for a survey page.
    /// </summary>
    [Serializable]
    public class SurveyPageSettings : PageSettings
    {
        public override ScreenType GetScreenType()
        {
            return ScreenType.Survey;
        }
    }

    /// <summary>
    /// The settings of a catch screen page.
    /// </summary>
    [Serializable]
    public class CatchScreenSettings : ScreenSettings, IVideoContainer
    {
        /// <summary>
        /// The path of the video file.
        /// </summary>
        [XmlElement("video")]
        public StringCommon videoPath;

        public override ScreenType GetScreenType()
        {
            return ScreenType.CatchScreen;
        }

        public StringCommon GetVideoPath()
        {
            return videoPath;
        }
    }

    /// <summary>
    /// The settings of a page with only text and no image/video.
    /// </summary>
    [Serializable]
    public class TextOnlyPageSettings : PageSettings, IAudioContainer
    {
        /// <summary>
        /// The text key of the content.
        /// </summary>
        [XmlElement("content")]
        public StringCommon content;
        /// <summary>
        /// The path of the audio file.
        /// </summary>
        [XmlElement("audio")]
        public StringCommon audioPath;

        public TextOnlyPageSettings() : base()
        {
        }

        public TextOnlyPageSettings(StringCommon title, StringCommon content) : base(title)
        {
            this.content = content;
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.TextOnly;
        }

        public StringCommon GetAudioPath()
        {
            return audioPath;
        }
    }

    /// <summary>
    /// The settings of a page for the choice of the player mode.
    /// </summary>
    [Serializable]
    public class PlayerModeSettings : PageSettings, IImageContainer
    {
        [XmlElement("p1_picto")]
        public StringCommon p1PictoPath;
        [XmlElement("p2_picto")]
        public StringCommon p2PictoPath;

        public PlayerModeSettings() : base()
        {
        }

        public PlayerModeSettings(StringCommon title) : base(title)
        {
        }

        public StringCommon[] GetImagePaths()
        {
            return new StringCommon[] { p1PictoPath, p2PictoPath };
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.PlayerMode;
        }
    }

    /// <summary>
    /// The settings of a page of contents, which is a page with a text and either a video or an image.
    /// </summary>
    [Serializable]
    public class ContentPageSettings : PageSettings, IAudioContainer, IVideoContainer, IImageContainer
    {
        public enum ContentScreenType
        {
            [XmlEnum("1")]
            Type1,
            [XmlEnum("2")]
            Type2,
        }
        /// <summary>
        /// The type of content page.
        /// </summary>
        [XmlAttribute("type")]
        public ContentScreenType contentScreenType;
        /// <summary>
        /// The text key of the content.
        /// </summary>
        [XmlElement("content")]
        public StringCommon content;
        /// <summary>
        /// The path of the image file.
        /// </summary>
        [XmlElement("image")]
        public StringCommon imagePath;
        /// <summary>
        /// The path of the video file.
        /// </summary>
        [XmlElement("video")]
        public StringCommon videoPath;
        /// <summary>
        /// The path of the audio file.
        /// </summary>
        [XmlElement("audio")]
        public StringCommon audioPath;

        public ContentPageSettings() : base()
        {
        }

        public ContentPageSettings(StringCommon title, ContentScreenType contentScreenType, StringCommon content, StringCommon imagePath, StringCommon videoPath) : base(title)
        {
            this.content = content;
            this.imagePath = imagePath;
            this.videoPath = videoPath;
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.ContentPage;
        }

        public StringCommon GetAudioPath()
        {
            return audioPath;
        }

        public StringCommon GetVideoPath()
        {
            return videoPath;
        }

        public StringCommon[] GetImagePaths()
        {
            return new StringCommon[] { imagePath };
        }
    }

    /// <summary>
    /// Generic settings of a page.
    /// </summary>
    [Serializable]
    public abstract class PageSettings : ScreenSettings
    {
        /// <summary>
        /// The text key of the title.
        /// </summary>
        [XmlElement("title")]
        public StringCommon title;
        /// <summary>
        /// Whether the next button should be displayed or not.
        /// </summary>
        [XmlAttribute("display_next")]
        public bool displayNext = true;
        /// <summary>
        /// The style of the next button.
        /// </summary>
        [XmlAttribute("next_style")]
        public int nextStyle = 1;

        public PageSettings()
        {
        }

        public PageSettings(StringCommon title)
        {
            this.title = title;
        }
    }

    public abstract class ScreenSettings
    {
        public enum ScreenType
        {
            ContentPage,
            PlayerMode,
            TextOnly,
            CatchScreen,
            Survey,
            ScoreScreen,
            BigScreen,
            Credits,
            Countdown,
        }

        public abstract ScreenType GetScreenType();

        public ScreenSettings()
        {
        }
    }
}