// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CRI.HitBox.Lang;
using CRI.HitBox.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UIFinalScoreScreen : UIScreen
    {
        /// <summary>
        /// The text of the final score.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the final score.")]
        private Text _finalScoreText = null;
        /// <summary>
        /// The text of the best score.
        /// </summary>
        [SerializeField]
        private Text _bestScoreText = null;
        /// <summary>
        /// The text of the ranking.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the ranking.")]
        private Text _rankingText = null;
        /// <summary>
        /// The path of the audio clip.
        /// </summary>
        [SerializeField]
        [Tooltip("The path of the audio clip.")]
        private StringCommon _audioClipPath;

        public Slider _precisionSlider;

        public Slider _speedSlider;

        protected override IEnumerator Start()
        {
            yield return base.Start();
            _audioClipPath = ((ScoreScreenSettings)GameManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == Settings.ScreenSettings.ScreenType.ScoreScreen)).audioPath;
            base.Start();
        }

        public float GetRating(float value)
        {
            int iValue = (int)value * 100;
            float res = 0.0f;
            int starWidth = 75;
            int sliderWidth = 500;
            int spaceWidth = (sliderWidth - 5 * starWidth) / 5;
            int stars = iValue / 20;

            res = (sliderWidth * iValue) + spaceWidth * stars / sliderWidth;
            return res;
        }

        public override void Show()
        {
            base.Show();
            int score = GameManager.instance.playerScore;
            GameMode mode = GameManager.instance.gameMode;
            _finalScoreText.text = _finalScoreText.text.Replace("[Var]", score.ToString());
            _bestScoreText.text = _bestScoreText.text.Replace("[Var]", GameManager.instance.GetBestScore(mode).ToString());
            _rankingText.text = _rankingText.text.Replace("[Var]", GameManager.instance.rank.ToString());
            if (!string.IsNullOrEmpty(_audioClipPath.key))
                AudioManager.instance.PlayClip(_audioClipPath.key, _audioClipPath.common);
        }

        public override void Hide()
        {
            base.Hide();
            if (!string.IsNullOrEmpty(_audioClipPath.key))
                AudioManager.instance.StopClip(_audioClipPath.key, _audioClipPath.common);
        }
    }
}