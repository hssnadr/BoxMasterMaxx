using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public struct PageSettings
{
    public enum PageType
    {
        [XmlEnum(Name = "PageType1")]
        PageType1,
        [XmlEnum(Name = "PageType2")]
        PageType2,
        [XmlEnum(Name = "ChoosePlayer")]
        ChoosePlayer,
        [XmlEnum(Name = "TextOnly")]
        TextOnly,
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
    /// The type of the page.
    /// </summary>
    [XmlAttribute("type")]
    public PageType pageType;
    /// <summary>
    /// The text key of the title.
    /// </summary>
    [XmlElement("title")]
    public TextKey title;
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

    public PageSettings(PageType pageType, TextKey title, TextKey content, string imagePath, string videoPath)
    {
        this.pageType = pageType;
        this.title = title;
        this.content = content;
        this.imagePath = imagePath;
        this.videoPath = videoPath;
    }
}