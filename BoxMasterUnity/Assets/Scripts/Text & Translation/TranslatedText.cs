// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A text UI that will be translated.
/// </summary>
[RequireComponent(typeof(Text))]
public class TranslatedText : MonoBehaviour
{
    /// <summary>
    /// The key to the text to translate.
    /// </summary>
    [Tooltip("The key to the text to translate.")]
    [SerializeField]
    protected string _textKey;

    /// <summary>
    /// The key of the text to translate.
    /// </summary>
    public string textKey
    {
        get
        {
            return _textKey;
        }
    }

    /// <summary>
    /// The text.
    /// </summary>
    [SerializeField]
    [Tooltip("The text.")]
    private Text _text;

    /// <summary>
    /// Is the text in the common file text ?
    /// </summary>
    [SerializeField]
    protected bool _isCommon = false;

    private void OnEnable()
    {
        TextManager.onLangChange += OnLangChange;
    }

    /// <summary>
    /// Called whenever the OnLangChange event of the TextManager is triggered. Sets the text to its current lang value.
    /// </summary>
    /// <param name="lang"></param>
    private void OnLangChange(LangApp lang)
    {
        SetText();
    }
    
    /// <summary>
    /// Init the translated text.
    /// </summary>
    /// <param name="textKey">The text key.</param>
    /// <param name="isCommon">Is the text common ?</param>
    public void InitTranslatedText(string textKey, bool isCommon=false)
    {
        _text = GetComponent<Text>();
        this._textKey = textKey;
        this._isCommon = isCommon;
        SetText();
    }

    /// <summary>
    /// Init the translated text.
    /// </summary>
    /// <param name="sc">A key / common pair.</param>
    public void InitTranslatedText(StringCommon sc)
    {
        InitTranslatedText(sc.key, sc.common);
    }

    /// <summary>
    /// Set the text to its translated value.
    /// </summary>
    private void SetText()
    {
        if (textKey == "")
            Debug.LogWarning("Missing Text Key");
        else if (_isCommon)
            _text.text = TextManager.instance.GetText(textKey, "COM");
        else
            _text.text = TextManager.instance.GetText(textKey);
    }

    private void Start()
    {
        _text = GetComponent<Text>();
        SetText();
    }

    private void OnDisable()
    {
        TextManager.onLangChange -= OnLangChange;
    }
}
