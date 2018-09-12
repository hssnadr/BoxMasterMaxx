// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Linq;
using CRI.HitBox.Settings;

namespace CRI.HitBox.UI
{
    public class UICatchScreen : UIScreen, IPointerClickHandler
    {
        /// <summary>
        /// The Screen Menu.
        /// </summary>
        [SerializeField]
        [Tooltip("The screen menu.")]
        protected UIScreenMenu _UIScreenMenu;

        [SerializeField]
        protected RawImage _videoTexture;

        public string videoClipPath = "";

        [SerializeField]
        public VideoPlayer _videoPlayer = null;

        protected override void Awake()
        {
            videoClipPath = ((CatchScreenSettings)GameManager.instance.menuSettings.pageSettings
                .First(x => x.GetScreenType() == ScreenSettings.ScreenType.CatchScreen)).videoPath.key;
            base.Awake();
            if (_UIScreenMenu == null)
                _UIScreenMenu = GetComponentInParent<UIScreenMenu>();
        }

        public override void Hide()
        {
            base.Hide();
            _videoPlayer.Stop();
        }

        public override void Show()
        {
            base.Show();
            _videoPlayer.Play();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _UIScreenMenu.menuBar.SetState(true);
        }

        public void Init(CatchScreenSettings screenSettings)
        {
            videoClipPath = screenSettings.videoPath.key;
            _videoPlayer.url = VideoManager.instance.GetCommonVideoPath(videoClipPath);
            _videoPlayer.targetTexture = (RenderTexture)_videoTexture.texture;
            _videoPlayer.Prepare();
        }
    }
}