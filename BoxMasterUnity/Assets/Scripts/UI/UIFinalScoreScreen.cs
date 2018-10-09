// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CRI.HitBox.Lang;
using CRI.HitBox.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UIFinalScoreScreen : UIScreen
    {
        [SerializeField]
        [Tooltip("The text of the title.")]
        private TranslatedText _titleText = null;
        /// <summary>
        /// The text of the label for the final score.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the label for the final score.")]
        private TranslatedText _finalScoreLabelText = null;
        /// <summary>
        /// The text of the value of the final score.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the final score")]
        private Text _finalScoreText = null;
        /// <summary>
        /// The text of the label of the precision.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the label of the precision.")]
        private TranslatedText _precisionLabelText = null;
        /// <summary>
        /// Slider of the precision.
        /// </summary>
        [SerializeField]
        [Tooltip("Slider of the precision.")]
        private Slider _precisionSlider = null;
        /// <summary>
        /// Fill of the precision.
        /// </summary>
        [SerializeField]
        [Tooltip("Fill of the precision.")]
        private Transform _precisionFill = null;
        /// <summary>
        /// The text of the label of the speed.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the label of the speed.")]
        private TranslatedText _speedLabelText = null;
        /// <summary>
        /// Slider of the speed.
        /// </summary>
        [SerializeField]
        [Tooltip("Slider of the speed.")]
        private Slider _speedSlider = null;
        /// <summary>
        /// Fill of the speed.
        /// </summary>
        [SerializeField]
        [Tooltip("Fill of the speed.")]
        private Transform _speedFill = null;
        /// <summary>
        /// The text of the label for the best score.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the label for the best score.")]
        private TranslatedText _bestScoreLabelText = null;
        /// <summary>
        /// The text of the value of the best score.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the value of the best score.")]
        private Text _bestScoreText = null;
        /// <summary>
        /// The text of the thanks message.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the thanks message.")]
        private TranslatedText _thanksText = null;
        /// <summary>
        /// The text of the points.
        /// </summary>
        [SerializeField]
        [Tooltip("The text of the points.")]
        public StringCommon _ptsText;

        /// <summary>
        /// The path of the audio clip.
        /// </summary>
        [SerializeField]
        [Tooltip("The path of the audio clip.")]
        private StringCommon _audioClipPath;

        private List<Vector3> _precisionStarPosition = new List<Vector3>();
        private List<Vector3> _speedStarPosition = new List<Vector3>();

        protected override IEnumerator Start()
        {
            var settings = (FinalScoreScreenSettings)ApplicationManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == Settings.ScreenSettings.ScreenType.FinalScoreScreen);
            _audioClipPath = settings.audioPath;
            _titleText.InitTranslatedText(settings.title);
            _finalScoreLabelText.InitTranslatedText(settings.scoreText);
            _precisionLabelText.InitTranslatedText(settings.precisionText);
            _speedLabelText.InitTranslatedText(settings.speedText);
            _bestScoreLabelText.InitTranslatedText(settings.bestScoreText);
            _thanksText.InitTranslatedText(settings.thanksText);
            _ptsText = settings.ptsText;
            
            foreach (Transform star in _precisionFill)
            {
                _precisionStarPosition.Add(star.position);
            }
            foreach (Transform star in _speedFill)
            {
                _speedStarPosition.Add(star.position);
            }
            yield return base.Start();
        }

        private float GetRating(float value, int starWidth, int sliderWidth)
        {
            float res = 0.0f;
            int spaceWidth = (sliderWidth - starWidth * 5) / 4;
            int starsCount = (int)(value * 5.0f);
            int starTotalWidth = starWidth * 5;
            res = ((starTotalWidth * value) + spaceWidth * starsCount) / sliderWidth;
            return Mathf.Clamp(res, (starWidth / 2.0f) / sliderWidth, 1.0f);
        }

        private string GetScoreText(int score, int fontSize, string ptsText)
        {
            var nfi = new NumberFormatInfo { NumberDecimalDigits = 0, NumberGroupSeparator = string.Format("<size={0}> </size>", (int)(fontSize * 0.3f)) };
            string formatted = score.ToString("n", nfi);
            return string.Format("{0} <size={1}>{2}</size>", formatted, (int)(fontSize * 0.4f), ptsText);
        }

        private void SetValues(int playerScore, float precision, float speed, int bestScore, string ptsText)
        {
            _finalScoreText.text = GetScoreText(
                playerScore,
                _finalScoreText.fontSize,
                ptsText
                );
            _precisionSlider.value = GetRating(
                precision,
                (int)_precisionFill.GetChild(0).GetComponent<RectTransform>().rect.width,
                (int)_precisionSlider.GetComponent<RectTransform>().rect.width
                );
            _speedSlider.value = GetRating(
                speed,
                (int)_speedFill.GetChild(0).GetComponent<RectTransform>().rect.width,
                (int)_speedSlider.GetComponent<RectTransform>().rect.width
                );
            _bestScoreText.text = GetScoreText(
                bestScore,
                _bestScoreText.fontSize,
                ptsText
                );
        }

        public override void Show()
        {
            base.Show();
            GameMode mode = ApplicationManager.instance.gameMode;
            SetValues(ApplicationManager.instance.gameManager.playerScore,
                ApplicationManager.instance.gameManager.precision,
                ApplicationManager.instance.gameManager.speed,
                ApplicationManager.instance.gameManager.GetBestScore(mode),
                TextManager.instance.GetText(_ptsText));
            if (!string.IsNullOrEmpty(_audioClipPath.key) && AudioManager.instance.isLoaded)
                AudioManager.instance.PlayClip(_audioClipPath.key, _audioClipPath.common);
        }

        public override void Hide()
        {
            base.Hide();
            if (!string.IsNullOrEmpty(_audioClipPath.key) && AudioManager.instance.isLoaded)
                AudioManager.instance.StopClip(_audioClipPath.key, _audioClipPath.common);
        }

        protected override void Update()
        {
            for (int i = 0; i < _precisionStarPosition.Count && _ready && _visible; i++)
            {
                _precisionFill.GetChild(i).position = _precisionStarPosition[i];
                _speedFill.GetChild(i).position = _speedStarPosition[i];
            }
        }
    }
}