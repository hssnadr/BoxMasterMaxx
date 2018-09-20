// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Lang;
using CRI.HitBox.Settings;

namespace CRI.HitBox.UI
{
    public class UISurveyButton : MonoBehaviour
    {
        public delegate void UISurveyButtonEvent(string str, SurveyAnswer.ButtonAction action, UISurveyButton button);
        public event UISurveyButtonEvent onAnswer;
        [SerializeField]
        protected Button _button;
        [SerializeField]
        protected TranslatedText _text;
        [SerializeField]
        protected TranslatedText _highlightedText;
        [SerializeField]
        protected Image _background;

        private Color _color;


        public void Init(SurveyAnswer answer, Color color)
        {
            _color = color;
            if (_button == null)
                _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(() =>
            {
                onAnswer(answer.key, answer.buttonAction, this);
            });

            _text.InitTranslatedText(answer.key);
            _highlightedText.InitTranslatedText(answer.key);
            _highlightedText.GetComponent<Text>().color = color;
            _background.color = color;
        }

        public void Highlight(bool highlight)
        {
            if (highlight)
            {
                _text.GetComponent<Text>().color = _color;
                _background.color = Color.black;
            }
            else
            {
                _text.GetComponent<Text>().color = Color.black;
                _background.color = _color;
            }
        }
    }
}