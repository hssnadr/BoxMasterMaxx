using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// The settings for a survey page.
/// </summary>
[Serializable]
public class SurveyPageSettings : PageSettings
{ 
    public override PageType GetPageType()
    {
        return PageType.Survey;
    }
}

/// <summary>
/// The settings of a catch screen page.
/// </summary>
[Serializable]
public class CatchScreenPageSettings : PageSettings
{
    /// <summary>
    /// The path of the video file.
    /// </summary>
    [XmlElement("video")]
    public string videoPath;

    public override PageType GetPageType()
    {
        return PageType.CatchScreen;
    }
}

/// <summary>
/// The settings of a page with only text and no image/video.
/// </summary>
[Serializable]
public class TextOnlyPageSettings : PageSettings
{
    /// <summary>
    /// The text key of the content.
    /// </summary>
    [XmlElement("content")]
    public TextKey content;
    /// <summary>
    /// The path of the audio file.
    /// </summary>
    [XmlElement("audio")]
    public string audioPath;

    public TextOnlyPageSettings() : base()
    {

    }

    public TextOnlyPageSettings(TextKey title, TextKey content) : base(title)
    {
        this.content = content;
    }

    public override PageType GetPageType()
    {
        return PageType.TextOnly;
    }
}

/// <summary>
/// The settings of a page for the choice of the player mode.
/// </summary>
[Serializable]
public class PlayerModeSettings : PageSettings
{
    public PlayerModeSettings() : base()
    {

    }

    public PlayerModeSettings(TextKey title) : base(title)
    {

    }

    public override PageType GetPageType()
    {
        return PageType.PlayerMode;
    }
}

/// <summary>
/// The settings of a page of contents, which is a page with a text and either a video or an image.
/// </summary>
[Serializable]
public class ContentPageSettings : PageSettings
{
    public enum ContentPageType
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
    public ContentPageType contentPageType;
    /// <summary>
    /// The text key of the content.
    /// </summary>
    [XmlElement("content")]
    public TextKey content;
    /// <summary>
    /// The path of the image file.
    /// </summary>
    [XmlElement("image")]
    public string imagePath;
    /// <summary>
    /// The path of the video file.
    /// </summary>
    [XmlElement("video")]
    public string videoPath;
    /// <summary>
    /// The path of the audio file.
    /// </summary>
    [XmlElement("audio")]
    public string audioPath;

    public ContentPageSettings() : base()
    {

    }

    public ContentPageSettings(TextKey title, ContentPageType contentPageType, TextKey content, string imagePath, string videoPath) : base(title)
    {
        this.content = content;
        this.imagePath = imagePath;
        this.videoPath = videoPath;
    }

    public override PageType GetPageType()
    {
        return PageType.ContentPage;
    }
}

/// <summary>
/// Generic settings of a page.
/// </summary>
[Serializable]
public abstract class PageSettings
{
    public enum PageType
    {
        ContentPage,
        PlayerMode,
        TextOnly,
        CatchScreen,
        Survey,
    }

    public struct TextKey
    {
        /// <summary>
        /// The key of the text entry
        /// </summary>
        [XmlText]
        public string key;
        /// <summary>
        /// Is a common text entry. If true, the page will look for the text key in the common text entry file. False by default.
        /// </summary>
        [XmlAttribute("common")]
        public bool common;

        public TextKey(string key, bool common = false)
        {
            this.key = key;
            this.common = common;
        }
    }
    /// <summary>
    /// The text key of the title.
    /// </summary>
    [XmlElement("title")]
    public TextKey title;
    /// <summary>
    /// Whether the next button should be displayed or not.
    /// </summary>
    [XmlAttribute("display_next")]
    public bool displayNext = true;

    public PageSettings()
    {

    }

    public PageSettings(TextKey title)
    {
        this.title = title;
    }

    public abstract PageType GetPageType();
}