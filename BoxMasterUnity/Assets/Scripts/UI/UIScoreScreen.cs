// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreScreen : MonoBehaviour, IHideable
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;
    [SerializeField]
    protected Text _player1Text;
    [SerializeField]
    protected Text _player2Text;
    [SerializeField]
    protected Text _timeText;

    private void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
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
    }

    private void Update()
    {
        int time = (int)(GameManager.instance.gameTime * 100);
        _player1Text.text = GameManager.instance.playerScore.ToString();
        _timeText.text = string.Format("{0:00}:{1:00}", time / 6000, (time / 100) % 60);
    }

    public bool HasNext()
    {
        return false;
    }

    public bool HasPrevious()
    {
        return false;
    }
}
