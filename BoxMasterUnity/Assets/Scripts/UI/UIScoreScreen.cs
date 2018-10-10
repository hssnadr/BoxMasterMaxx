// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CRI.HitBox.Lang;
using CRI.HitBox.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private StringCommon _countdownAudioPath;
        private StringCommon _countdownEndAudioPath;
        private int _countdownStartingPoint;
        private bool _countdownStarted = false;

        private bool _visible;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            Hide();
        }

        private IEnumerator Start()
        {
            if (GetComponentInParent<UIScreenMenu>() != null)
            {
                while (!GetComponentInParent<UIScreenMenu>().loaded)
                    yield return null;
            }
            var settings = (ScoreScreenSettings)ApplicationManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.ScoreScreen);
            _countdownAudioPath = settings.finalCountdownAudioPath;
            _countdownStartingPoint = settings.finalCountdownStartingPoint;
            _countdownEndAudioPath = settings.finalCountdownEndAudioPath;
        }

        public void Hide()
        {
            _visible = false;
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _countdownStarted = false;
        }

        public void Show()
        {
            _visible = true;
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _countdownStarted = false;
        }

        private void Update()
        {
            if (_visible)
            {
                int time = (int)(Mathf.Clamp(ApplicationManager.instance.timeLeft * 100, 0, 360000));
                _scoreText.text = ApplicationManager.instance.gameManager.playerScore.ToString();
                _comboText.text = string.Format("x{0}", ApplicationManager.instance.gameManager.comboMultiplier.ToString());
                _timeText.text = string.Format("{0:00}:{1:00}", (time / 6000) % 60, (time / 100) % 60);
                _comboBar.value = ApplicationManager.instance.gameManager.comboValue;
                if (!_countdownStarted && ((int)ApplicationManager.instance.timeLeft == _countdownStartingPoint) && !string.IsNullOrEmpty(_countdownAudioPath.key))
                {
                    AudioManager.instance.PlayClip(_countdownAudioPath.key, _countdownAudioPath.common);
                    _countdownStarted = true;
                }
                if (_countdownStarted && ((int)ApplicationManager.instance.timeLeft == 0) && !string.IsNullOrEmpty(_countdownEndAudioPath.key))
                {
                    AudioManager.instance.PlayClip(_countdownEndAudioPath.key, _countdownEndAudioPath.common);
                    _countdownStarted = false;
                }
            }
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