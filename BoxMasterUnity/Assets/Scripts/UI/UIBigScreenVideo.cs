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

        protected override IEnumerator Start()
        {
            yield return base.Start();
            string videoClipPath = ((BigScreenSettings)GameManager.instance.menuSettings.screenSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.BigScreen)).videoPath.key;
            string url = VideoManager.instance.GetCommonVideoPath(videoClipPath);
            _videoPlayer.url = url;
            _videoPlayer.targetTexture = (RenderTexture)GetComponent<RawImage>().texture;
            _videoPlayer.Prepare();
            Show();
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