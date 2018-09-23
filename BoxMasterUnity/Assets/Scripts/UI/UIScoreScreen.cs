// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
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
        private Slider _comboBar = null;
        [SerializeField]
        private Image[] _buttons;

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
            int time = (int)(Mathf.Clamp(ApplicationManager.instance.timeLeft * 100, 0, 360000));
            _scoreText.text = ApplicationManager.instance.gameManager.playerScore.ToString();
            _comboText.text = "x" + ApplicationManager.instance.gameManager.comboMultiplier.ToString();
            _timeText.text = string.Format("{0:00}:{1:00}", (time / 6000) % 60, (time / 100) % 60);
            _comboBar.value = ApplicationManager.instance.gameManager.comboValue;
        }

        public bool HasNext(out int nextStyle)
        {
            nextStyle = 0;
            return false;
        }

        public bool HasPrevious()
        {
            return false;
        }
    }
}