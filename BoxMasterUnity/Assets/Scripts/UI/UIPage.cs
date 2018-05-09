// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class UIPage : MonoBehaviour, IHideable, IPointerClickHandler
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected Animator _backButtonAnimator;

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
    }

    public void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _backButtonAnimator.SetTrigger("Normal");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<UIScreenMenu>().GoToNext();
    }
}
