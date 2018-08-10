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
    protected Text _scoreText;
    [SerializeField]
    protected Text _timeText;
    [SerializeField]
    private Text _comboText = null;
    [SerializeField]
    private Transform _comboBar = null;
    [SerializeField]
    private Image _comboBarButtonPrefab = null;
    private Image[] _buttons;

    private void Awake()
    {
        if (_canvasGroup == null)
            _canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    private void Start()
    {
        _buttons = new Image[GameManager.instance.gameSettings.comboMultiplierThreshold];
        for (int i = 0; i < GameManager.instance.gameSettings.comboMultiplierThreshold; i++)
        {
            var go = GameObject.Instantiate(_comboBarButtonPrefab, _comboBar);
            _buttons[i] = go;
        }
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
        _scoreText.text = GameManager.instance.playerScore.ToString();
        _comboText.text = "x" + GameManager.instance.comboCount.ToString();
        _timeText.text = string.Format("{0:00}:{1:00}", time / 6000, (time / 100) % 60);

        for (int i = 0; i < GameManager.instance.gameSettings.comboMultiplierThreshold; i++)
        {
            _buttons[i].enabled = (i < GameManager.instance.comboCount);
        }
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
