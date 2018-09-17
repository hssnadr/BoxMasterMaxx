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

        /// <summary>
        /// The text above the video.
        /// </summary>
        [SerializeField]
        [Tooltip("The text above the video.")]
        private Text _videoText = null;

        protected override IEnumerator Start()
        {
            yield return base.Start();
            var settings = (BigScreenSettings)GameManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.BigScreen);
            string videoClipPath = settings.videoPath.key;
            string url = VideoManager.instance.GetCommonVideoPath(videoClipPath);
            _videoPlayer.url = url;
            _videoPlayer.targetTexture = (RenderTexture)GetComponent<RawImage>().texture;
            _videoTextKey = settings.text.key;
            _videoPlayer.prepareCompleted += (val) =>
            {
                _videoDuration = _videoPlayer.frameCount / _videoPlayer.frameRate;
                InitLangSequence(_videoTextKey, GameManager.instance.gameSettings, TextManager.instance);
                StartCoroutine(VideoTextAnimation());
            };
            _videoPlayer.Prepare();
            Show();
        }

        private void InitLangSequence(string textKey, GameSettings settings, TextManager textManager)
        {
            List<LangApp> langs = settings.langAppEnable.ToList();
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
        }

        private IEnumerator VideoTextAnimation()
        {
            int index = 0;
            _videoText.text = textSequence[0];
            if (textSequence.Count > 1)
            {
                while (this.enabled)
                {
                    _videoText.text = textSequence[index];
                    yield return new WaitForSeconds(Mathf.Max((_videoDuration / textSequence.Count) - 0.5f, 0));
                    if (_videoText.GetComponent<Animator>() != null)
                    {
                        _videoText.GetComponent<Animator>().SetTrigger("Activate");
                    }
                    yield return new WaitForSeconds(0.5f);
                    index = (index + 1) % textSequence.Count;
                }
            }
        }

        public override void Show()
        {
            base.Show();
            _videoPlayer.Play();
        }

        public override void Hide()
        {
            base.Hide();
            _videoPlayer.Stop();
        }
    }
}