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
    public class Gif
    {
        /// <summary>
        /// Framerate of the gif.
        /// </summary>
        [XmlAttribute("framerate")]
        public int framerate;
        /// <summary>
        /// Is this gif found in the common folder ?
        /// </summary>
        [XmlAttribute("common")]
        public bool common;
        /// <summary>
        /// First index of the gif.
        /// </summary>
        [XmlAttribute("first_index")]
        public int firstIndex;
        /// <summary>
        /// Last index of the gif.
        /// </summary>
        [XmlAttribute("last_index")]
        public int lastIndex;
        /// <summary>
        /// Path of the gif.
        /// </summary>
        [XmlText]
        public string path;
        
        public StringCommon[] GetFrames()
        {
            var res = new StringCommon[(lastIndex - firstIndex) + 1];
            for (int i = 0; i < res.Length; i++)
            {
                res[i] = new StringCommon(path.Replace("{id}", (firstIndex + i).ToString()), common);
            }
            return res;
        }
    }
    [Serializable]
    public class CountdownSettings : ScreenSettings, IAudioContainer
    {
        /// <summary>
        /// The path of the audio file.
        /// </summary>
        [XmlElement("countdown_audio")]
        public StringCommon countdownAudioPath;
        /// <summary>
        /// The path of the "go!" audio file.
        /// </summary>
        [XmlElement("go_audio")]
        public StringCommon goAudioPath;
        /// <summary>
        /// The text at the end of the countdown.
        /// </summary>
        [XmlElement("text")]
        public StringCommon text;
        /// <summary>
        /// The countdown time.
        /// </summary>
        [XmlElement("countdown_time")]
        public int countdownTime;

        public StringCommon[] GetAudioPaths()
        {
            return new StringCommon[] { countdownAudioPath, goAudioPath };
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
        /// <summary>
        /// Prefered minimal height or width of the texture.
        /// </summary>
        [XmlElement("prefered_min_texture_size")]
        public int preferedMinTextureSize;

        public StringCommon[] GetImagePaths()
        {
            if (leftContent.logos == null && rightContent.logos != null)
                return rightContent.logos;
            if (leftContent.logos != null && rightContent.logos == null)
                return leftContent.logos;
            if (rightContent.logos != null && rightContent.logos != null)
                return leftContent.logos.Concat(rightContent.logos).ToArray();
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
        [XmlElement("text")]
        public StringCommon text;
        /// <summary>
        /// The path of the video file.
        /// </summary>
        [XmlElement("video")]
        public StringCommon videoPath;

        public StringCommon[] GetVideoPaths()
        {
            return new StringCommon[] { videoPath };
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.BigScreen;
        }
    }

    [Serializable]
    public class ScoreScreenSettings : ScreenSettings, IAudioContainer
    {
        /// <summary>
        /// The audio path of the final countdown.
        /// </summary>
        [XmlElement("final_countdown_audio")]
        public StringCommon finalCountdownAudioPath;
        /// <summary>
        /// The audio path of the end of the final countdown.
        /// </summary>
        [XmlElement("final_countdown_end_audio")]
        public StringCommon finalCountdownEndAudioPath;
        /// <summary>
        /// The starting point of the final countdown.
        /// Exemple : if the starting point is set to 5, the final countdown audio will start 5 seconds before the end.
        /// </summary>
        [XmlElement("final_countdown_starting_point")]
        public int finalCountdownStartingPoint;

        public StringCommon[] GetAudioPaths()
        {
            return new StringCommon[] { finalCountdownAudioPath, finalCountdownEndAudioPath };
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.ScoreScreen;
        }
    }

    /// <summary>
    /// The settings for the score screen.
    /// </summary>
    [Serializable]
    public class FinalScoreScreenSettings : ScreenSettings, IAudioContainer
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
        /// <summary>
        /// Text for the points.
        /// </summary>
        [XmlElement("pts_text")]
        public StringCommon ptsText;

        public StringCommon[] GetAudioPaths()
        {
            return new StringCommon[] { audioPath };
        }

        public override ScreenType GetScreenType()
        {
            return ScreenType.FinalScoreScreen;
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

        public StringCommon[] GetVideoPaths()
        {
            return new StringCommon[] { videoPath };
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

        public StringCommon[] GetAudioPaths()
        {
            return new StringCommon[] { audioPath };
        }
    }

    /// <summary>
    /// The settings of a page for the choice of the player mode.
    /// </summary>
    [Serializable]
    public class PlayerModeSettings : PageSettings, IImageContainer
    {
        /// <summary>
        /// Path of the picto of the solo mode.
        /// </summary>
        [XmlElement("p1_picto")]
        public StringCommon p1PictoPath;
        /// <summary>
        /// Path of the gif of the solo mode.
        /// </summary>
        [XmlElement("p1_gif")]
        public Gif p1Gif;
        /// <summary>
        /// Path of the picto of the coop mode.
        /// </summary>
        [XmlElement("p2_picto")]
        public StringCommon p2PictoPath;
        /// <summary>
        /// Path of the gif of the solo mode.
        /// </summary>
        [XmlElement("p2_gif")]
        public Gif p2Gif;

        public PlayerModeSettings() : base()
        {
        }

        public PlayerModeSettings(StringCommon title) : base(title)
        {
        }

        public StringCommon[] GetImagePaths()
        {
            var res = new StringCommon[] { p1PictoPath, p2PictoPath };
            if (p1Gif != null)
                res = res.Concat(p1Gif.GetFrames()).ToArray();
            if (p2Gif != null)
                res = res.Concat(p2Gif.GetFrames()).ToArray();
            return res;
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
        /// <summary>
        /// A gif.
        /// </summary>
        [XmlElement("gif")]
        public Gif gif;

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

        public StringCommon[] GetAudioPaths()
        {
            return new StringCommon[] { audioPath };
        }

        public StringCommon[] GetVideoPaths()
        {
            return new StringCommon[] { videoPath };
        }

        public StringCommon[] GetImagePaths()
        {
            var res = new StringCommon[] { imagePath };
            if (gif != null)
            {
                res = res.Concat(gif.GetFrames()).ToArray();
            }
            return res;
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
            FinalScoreScreen,
            BigScreen,
            Credits,
            Countdown,
            ScoreScreen,
        }

        public abstract ScreenType GetScreenType();

        public ScreenSettings()
        {
        }
    }
}