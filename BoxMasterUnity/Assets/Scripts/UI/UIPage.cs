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
public class UIPage : MonoBehaviour, IHideable, IPointerClickHandler
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected Animator _backButtonAnimator;
    [SerializeField]
    protected TranslatedText _title;
    [SerializeField]
    protected TranslatedText _content;
    [SerializeField]
    protected RawImage _rawImage;
    [SerializeField]
    protected RawImage _videoTexture;

    public string videoClipPath = "";

    public TranslatedText title
    {
        get { return _title; }
    }

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

    private void Start()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        if (_backButtonAnimator == null)
            _backButtonAnimator = GetComponentInChildren<Animator>();
        Hide();
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _backButtonAnimator.SetTrigger("Normal");
        VideoManager.instance.StopClip();
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _backButtonAnimator.SetTrigger("Normal");
        if (videoClipPath != "" && _videoTexture.enabled)
            VideoManager.instance.PlayClip(videoClipPath, (RenderTexture)_videoTexture.texture);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<UIScreenMenu>().GoToNext();
    }
}
