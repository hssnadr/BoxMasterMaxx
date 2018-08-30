using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public interface IAudioContainer
{
    PageSettings.StringCommon GetAudioPath();
}

public interface IVideoContainer
{
    PageSettings.StringCommon GetVideoPath();
}

public interface IImageContainer
{
    PageSettings.StringCommon[] GetImagePaths();
}

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
public class CatchScreenPageSettings : PageSettings, IVideoContainer
{
    /// <summary>
    /// The path of the video file.
    /// </summary>
    [XmlElement("video")]
    public StringCommon videoPath;

    public override PageType GetPageType()
    {
        return PageType.CatchScreen;
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

    public override PageType GetPageType()
    {
        return PageType.TextOnly;
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

    public override PageType GetPageType()
    {
        return PageType.PlayerMode;
    }
}

/// <summary>
/// The settings of a page of contents, which is a page with a text and either a video or an image.
/// </summary>
[Serializable]
public class ContentPageSettings : PageSettings, IAudioContainer, IVideoContainer, IImageContainer
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

    public ContentPageSettings(StringCommon title, ContentPageType contentPageType, StringCommon content, StringCommon imagePath, StringCommon videoPath) : base(title)
    {
        this.content = content;
        this.imagePath = imagePath;
        this.videoPath = videoPath;
    }

    public override PageType GetPageType()
    {
        return PageType.ContentPage;
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

    public struct StringCommon
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

        public StringCommon(string key, bool common = false)
        {
            this.key = key;
            this.common = common;
        }
    }
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

    public abstract PageType GetPageType();
}