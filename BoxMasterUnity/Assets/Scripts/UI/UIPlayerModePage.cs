// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Settings;

namespace CRI.HitBox.UI
{
    public class UIPlayerModePage : UIPage<PlayerModeSettings>
    {
        [SerializeField]
        private Button _p1Button = null;
        [SerializeField]
        private Button _p2Button = null;
        [SerializeField]
        private RawImage _p1Picto = null;
        [SerializeField]
        private RawImage _p2Picto = null;
        private Texture[] _framesP1;
        private Texture[] _framesP2;
        private int _frameIndexP1;
        private int _frameIndexP2;
        private int _framerateP1;
        private int _framerateP2;
        private float _timeShow;


        protected override void Awake()
        {
            base.Awake();
            Hide();
        }

        protected void Start()
        {
            _p1Button.onClick.AddListener(() =>
            {
                ApplicationManager.instance.SetGameMode(GameMode.P1);
                GetComponentInParent<UIScreenMenu>().GoToNext();
            });

            _p2Button.onClick.AddListener(() =>
            {
                ApplicationManager.instance.SetGameMode(GameMode.P2);
                GetComponentInParent<UIScreenMenu>().GoToNext();
            });
        }

        public override void Hide()
        {
            base.Hide();
            _p1Button.GetComponent<Animator>().SetTrigger("Normal");
            _p2Button.GetComponent<Animator>().SetTrigger("Normal");
            _frameIndexP1 = 0;
            _frameIndexP2 = 0;
        }

        public override void Show()
        {
            base.Show();
            _p1Button.GetComponent<Animator>().SetTrigger("Normal");
            _p2Button.GetComponent<Animator>().SetTrigger("Normal");
            _frameIndexP1 = 0;
            _frameIndexP2 = 0;
            _timeShow = Time.time;
        }

        public override void Init(PlayerModeSettings playerModeSettings)
        {
            base.Init(playerModeSettings);
            if (!String.IsNullOrEmpty(playerModeSettings.p1PictoPath.key) && TextureManager.instance.HasTexture(playerModeSettings.p1PictoPath.key))
                _p1Picto.texture = TextureManager.instance.GetTexture(playerModeSettings.p1PictoPath);
            if (!String.IsNullOrEmpty(playerModeSettings.p2PictoPath.key) && TextureManager.instance.HasTexture(playerModeSettings.p2PictoPath.key))
                _p2Picto.texture = TextureManager.instance.GetTexture(playerModeSettings.p2PictoPath);
            if (playerModeSettings.p1Gif != null)
            {
                _framesP1 = LoadGif(playerModeSettings.p1Gif);
                _framerateP1 = playerModeSettings.p1Gif.framerate;
            }
            if (playerModeSettings.p2Gif != null)
            {
                _framesP2 = LoadGif(playerModeSettings.p2Gif);
                _framerateP2 = playerModeSettings.p2Gif.framerate;
            }
        }

        private void Update()
        {
            if (_framesP1 != null && _framesP1.Length > 0 && _visible)
            {
                _frameIndexP1 = ((int)((Time.time - _timeShow) * _framerateP1) % _framesP1.Length);
                _p1Picto.texture = _framesP1[_frameIndexP1];
            }
            if (_framesP2 != null && _framesP2.Length > 0 && _visible)
            {
                _frameIndexP2 = ((int)((Time.time - _timeShow) * _framerateP2) % _framesP2.Length);
                _p2Picto.texture = _framesP2[_frameIndexP2];
            }
        }
    }
}