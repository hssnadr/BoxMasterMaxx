// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Settings;

namespace CRI.HitBox.UI
{
    public class UILangMenu : MonoBehaviour
    {
        [SerializeField]
        protected UILangSelectButton _startLangButtonPrefab;
        [SerializeField]
        protected GameObject _copyrightButtonPrefab;
        [SerializeField]
        protected GameObject _soundButtonPrefab;
        [SerializeField]
        protected GameObject _separatorPrefab;

        protected List<GameObject> _buttons = new List<GameObject>();

        private void Start()
        {
            CreateButtons();
        }

        public void CreateButtons()
        {
            foreach (GameObject button in _buttons)
            {
                Destroy(button);
            }
            _buttons.Clear();
            foreach (var buttonType in GameManager.instance.menuSettings.menuBarLayout)
            {
                switch (buttonType)
                {
                    case ButtonType.Start:
                        CreateLangButtons();
                        break;
                    case ButtonType.Copyright:
                        CreateCopyrightButton();
                        break;
                    case ButtonType.Sound:
                        CreateSoundButton();
                        break;
                    case ButtonType.Separator:
                        CreateSeparatorButton();
                        break;
                }
            }
        }

        private void CreateLangButtons()
        {
            foreach (var lang in GameManager.instance.gameSettings.langAppEnable)
            {
                var langSelectButton = GameObject.Instantiate(_startLangButtonPrefab, this.transform);
                langSelectButton.lang = lang;
                _buttons.Add(langSelectButton.gameObject);
            }
        }

        private void CreateCopyrightButton()
        {
            var go = GameObject.Instantiate(_copyrightButtonPrefab, this.transform);
            _buttons.Add(go);
        }

        private void CreateSoundButton()
        {
            var go = GameObject.Instantiate(_soundButtonPrefab, this.transform);
            _buttons.Add(go);
        }

        private void CreateSeparatorButton()
        {
            var go = GameObject.Instantiate(_separatorPrefab, this.transform);
            _buttons.Add(go);
        }
    }
}