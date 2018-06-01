// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILangSelectButton : MonoBehaviour
{
    public LangApp lang;

    public string textKey;

    [SerializeField]
    protected Button _button;
    [SerializeField]
    protected Text _text;
    [SerializeField]
    protected Text _highlightedText;
    [SerializeField]
    protected Image _background;
    [SerializeField]
    protected Animator _animator;

    protected UIScreenMenu _UIScreenMenu;

    private void Start()
    {
        if (_animator == null)
            _animator = GetComponent<Animator>();

        _UIScreenMenu = GetComponentInParent<UIScreenMenu>();

        if (_button == null)
            _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(() =>
           {
               TextManager.instance.currentLang = lang;
               _UIScreenMenu.GoToFirstPage();
           });

        _background.color = lang.color;

        if (_text != null)
        {
            _text.text = TextManager.instance.GetText(textKey, lang.code);
            _text.fontStyle = GameManager.instance.gameSettings.defaultLanguage.Equals(lang) ? FontStyle.Bold : FontStyle.Normal;
        }
        if (_highlightedText != null)
        {
            _highlightedText.text = TextManager.instance.GetText(textKey, lang.code);
            _highlightedText.fontStyle = GameManager.instance.gameSettings.defaultLanguage.Equals(lang) ? FontStyle.Bold : FontStyle.Normal;
            _highlightedText.color = lang.color;
        }
    }
}