// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CRI.HitBox.UI
{
    public class UIBigScreen : MonoBehaviour
    {
        /// <summary>
        /// The screen that displays the score.
        /// </summary>
        [SerializeField]
        [Tooltip("The screen that displays the score.")]
        private UIScoreScreen _scoreScreen = null;
        /// <summary>
        /// The page displayed for the countdown.
        /// </summary>
        [SerializeField]
        [Tooltip("The page displayed for the countdown.")]
        private UICountdownPage _countdownPage = null;
        /// <summary>
        /// The video displayed on the big screen.
        /// </summary>
        [SerializeField]
        [Tooltip("The video displayed on the big screen.")]
        private UIBigScreenVideo _video = null;
        /// <summary>
        /// The screen that display the final score.
        /// </summary>
        [SerializeField]
        [Tooltip("The screen that display the final score.")]
        private UIFinalScoreScreen _finalScoreScreen = null;

        private void OnEnable()
        {
            ApplicationManager.onSetupEnd += OnSetupEnd;
            ApplicationManager.onStartPages += OnStartPages;
            ApplicationManager.onReturnToHome += OnReturnToHome;
            ApplicationManager.onGameStart += OnGameStart;
            ApplicationManager.onGameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            ApplicationManager.onSetupEnd -= OnSetupEnd;
            ApplicationManager.onStartPages -= OnStartPages;
            ApplicationManager.onReturnToHome -= OnReturnToHome;
            ApplicationManager.onGameStart -= OnGameStart;
            ApplicationManager.onGameEnd -= OnGameEnd;
        }

        private void Start()
        {
            OnReturnToHome(HomeOrigin.Quit);
        }

        private void OnGameStart(GameMode gameMode, int soloIndex)
        {
            _countdownPage.Hide();
            _scoreScreen.Show();
            _finalScoreScreen.Hide();
            _video.Hide();
        }

        private void OnStartPages(bool switchLanguages)
        {
            OnReturnToHome(HomeOrigin.Quit);
        }

        private void OnReturnToHome(HomeOrigin homeOrigin)
        {
            _countdownPage.Hide();
            _scoreScreen.Hide();
            _finalScoreScreen.Hide();
            _video.Show();
        }

        private void OnSetupEnd()
        {
            _countdownPage.Show();
            _scoreScreen.Hide();
            _finalScoreScreen.Hide();
            _video.Hide();
        }

        private void OnGameEnd()
        {
            _countdownPage.Hide();
            _scoreScreen.Hide();
            _finalScoreScreen.Show();
            _video.Hide();
        }
    }
}
