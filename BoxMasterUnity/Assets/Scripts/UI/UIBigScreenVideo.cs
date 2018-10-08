// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using CRI.HitBox.Settings;
using CRI.HitBox.Lang;

namespace CRI.HitBox.UI
{
    public class UIBigScreenVideo : UIScreen
    {
        /// <summary>
        /// The video player.
        /// </summary>
        [SerializeField]
        [Tooltip("The video player.")]
        private VideoPlayer _videoPlayer = null;

        private float _videoDuration;

        private string _videoTextKey;

        private List<string> textSequence = new List<string>();

        private bool _completed = false;

        private float _time;

        private bool _waitForAnimation = false;

        /// <summary>
        /// The text above the video.
        /// </summary>
        [SerializeField]
        [Tooltip("The text above the video.")]
        private Text _videoText = null;

        private int _index = 0;

        private float _interval;
        private float _interval2;

        protected override IEnumerator Start()
        {
            yield return base.Start();
            var settings = (BigScreenSettings)ApplicationManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.BigScreen);
            string videoClipPath = settings.videoPath.key;
            string url = VideoManager.instance.GetCommonVideoPath(videoClipPath);
            _videoPlayer.url = url;
            _videoPlayer.targetTexture = (RenderTexture)GetComponent<RawImage>().texture;
            _videoTextKey = settings.text.key;
            InitLangSequence(_videoTextKey, ApplicationManager.instance.appSettings, TextManager.instance);
            _videoText.text = textSequence[0];

            _videoPlayer.prepareCompleted += (val) =>
            {
                _videoDuration = _videoPlayer.frameCount / _videoPlayer.frameRate;
                _interval = Mathf.Max((_videoDuration / textSequence.Count) - 0.5f, 0);
                _interval2 = 0.5f;
                _time = Time.time;
                _completed = true;
            };
            _videoPlayer.Prepare();
            Show();
        }

        private List<string> InitLangSequence(string textKey, ApplicationSettings settings, TextManager textManager)
        {
            List<LangApp> langs = settings.langAppEnable.ToList();
            textSequence = new List<string>();
            if (langs.Distinct().Count() == 1)
            {
                textSequence.Add(textManager.GetText(textKey, langs[0].code));
            }
            else if (langs.Distinct().Count() > 1)
            {
                foreach (var lang in langs)
                {
                    if (lang.code != settings.defaultLanguage.code)
                    {
                        textSequence.Add(textManager.GetText(textKey, settings.defaultLanguage.code));
                        textSequence.Add(textManager.GetText(textKey, lang.code));
                    }
                }
            }
            return textSequence; 
        }

        public override void Show()
        {
            base.Show();
            _videoPlayer.Play();
            _time = Time.time;
            _waitForAnimation = false;
            _index = 0;
            _videoText.text = textSequence[0];
        }

        public override void Hide()
        {
            base.Hide();
            _videoPlayer.Stop();
        }

        protected override void Update()
        {
            if (textSequence.Count > 1 && visible && _completed)
            {
                if (Time.time - _time > _interval && _videoText.GetComponent<Animator>() != null && !_waitForAnimation)
                {
                    _videoText.GetComponent<Animator>().SetTrigger("Activate");
                    _waitForAnimation = true;
                    _time = Time.time;
                }
                else if (Time.time - _time > _interval2 && _waitForAnimation)
                {
                    _index = (_index + 1) % textSequence.Count;
                    _videoText.text = textSequence[_index];
                    _waitForAnimation = false;
                    _time = Time.time;
                }
            }
        }
    }
}