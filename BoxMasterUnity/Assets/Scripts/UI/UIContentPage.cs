// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIContentPage : UIPage
{
    [SerializeField]
    protected TranslatedText _content;
    [SerializeField]
    protected RawImage _rawImage;
    [SerializeField]
    protected RawImage _videoTexture;

    /// <summary>
    /// The path of the video clip that will be played when the path is shown.
    /// </summary>
    [Tooltip("The path of the video clip that will be played when the path is shown.")]
    public string videoClipPath = "";
    /// <summary>
    /// The path of the audio clip that will be played when the path is shown.
    /// </summary>
    [Tooltip("The path of the audio clip that will be played when the page is shown.")]
    public string audioClipPath = "";

    public TranslatedText content
    {
        get { return _content; }
    }

    public RawImage rawImage
    {
        get { return _rawImage; }
    }

    public RawImage videoTexture
    {
        get { return _videoTexture;  }
    }

    public override void Hide()
    {
        base.Hide();
        if (videoClipPath != "" && _videoTexture.enabled)
            VideoManager.instance.StopClip(videoClipPath);
        if (audioClipPath != "")
            AudioManager.instance.StopClip(audioClipPath);
    }

    public override void Show()
    {
        base.Show();
        if (videoClipPath != "" && _videoTexture.enabled)
            VideoManager.instance.PlayClip(videoClipPath, (RenderTexture)_videoTexture.texture);
        if (audioClipPath != "")
            AudioManager.instance.PlayClip(audioClipPath);
    }
}
