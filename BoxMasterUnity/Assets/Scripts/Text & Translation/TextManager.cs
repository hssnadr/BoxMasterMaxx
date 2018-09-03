// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System;

/// <summary>
/// Manages all the texts of the application.
/// </summary>
public class TextManager : MonoBehaviour
{
    public delegate void TextManagerLangHandler(LangApp lang);
    public static event TextManagerLangHandler onLangChange;

    /// <summary>
    /// Static instance of the TextManager. If it's called an no TextManager exists, creates one.
    /// </summary>
    public static TextManager instance
    {
        get
        {
            if (s_instance == null)
            {
                new GameObject("TextManager").AddComponent<TextManager>().Init();
            }
            return s_instance;
        }
    }

    /// <summary>
    /// Static instance of the TextManager.
    /// </summary>
    private static TextManager s_instance = null;

    /// <summary>
    /// The path of the text data for each language. The [lang_app] value will be replaced by the code of the language. For exemple for French, it will be "fr".
    /// </summary>
    public const string text_lang_path_base = "lang/[lang_app]/text/text.xml";
    /// <summary>
    /// The path of the text data for all the text that is common between all languages.
    /// </summary>
    public const string text_lang_common_path = "lang/Common/text/text.xml";
    /// <summary>
    /// The current language of the application.
    /// </summary>
    private LangApp _currentLang;
    /// <summary>
    /// The current language of the application. If the value is changed, it will trigger the OnLangChange event.
    /// </summary>
    public LangApp currentLang
    {
        get
        {
            return _currentLang;
        }
        set
        {
            _currentLang = value;
            if (onLangChange != null)
                onLangChange(_currentLang);
        }
    }
    /// <summary>
    /// A list of all the LangText.
    /// </summary>
    public List<LangText> langTextList = new List<LangText>();

    private void OnEnable()
    {
        GameManager.onReturnToHome += OnReturnHome;
    }

    private void OnDisable()
    {
        GameManager.onReturnToHome -= OnReturnHome;
    }

    private void OnReturnHome()
    {
        SetDefaultLang();
    }

    private void Awake()
    {
        Init();
        foreach (var langEnable in GameManager.instance.gameSettings.langAppEnable)
        {
            var langText = LoadLangText(langEnable.code);
            langTextList.Add(langText);
        }
        var commonText = LoadCommonLangText();
        langTextList.Add(commonText);
        _currentLang = GameManager.instance.gameSettings.defaultLanguage;
    }

    private void Init()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (s_instance != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// Sets the current language to the default language of the application as described in the game settings.
    /// </summary>
    public void SetDefaultLang()
    {
        currentLang = GameManager.instance.gameSettings.defaultLanguage;
    }

    /// <summary>
    /// Get the path of the text of a language.
    /// </summary>
    /// <param name="langCode">The code of the language.</param>
    /// <returns>The path to find the text of that language.</returns>
    public string GetLangTextPath(string langCode)
    {
        var path = text_lang_path_base.Replace("[lang_app]", langCode);
        return path;
    }

    /// <summary>
    /// Loads the text that is common for all languages.
    /// </summary>
    /// <returns>A LangText with all the common data.</returns>
    public LangText LoadCommonLangText()
    {
        return LangText.Load(Path.Combine(Application.streamingAssetsPath, text_lang_common_path));
    }
    /// <summary>
    /// Loads the text for a specific language.
    /// </summary>
    /// <param name="langCode">The code of the language.</param>
    /// <returns>A LangText will all the data for that language.</returns>
    public LangText LoadLangText(string langCode)
    {
        return LangText.Load(Path.Combine(Application.streamingAssetsPath, GetLangTextPath(langCode)));
    }

    /// <summary>
    /// Finds the text of a language by using a specific key and a lang code.
    /// Exemple: For the key "SCREEN2_TITLE" and the lang "fr" it will return "REGLES DU JEU."
    /// For the key "SCREEN2_TITLE" and the lang "en" it will return "GAME RULES."
    /// </summary>
    /// <param name="key">The key of the text.</param>
    /// <param name="langCode">The code of the language.</param>
    /// <returns>The text translated to a specific language.</returns>
    public string GetText(string key, string langCode)
    {
        string res = "";
        try
        {
            res = langTextList.First(x => x.code == langCode).arrayOfLangTextEntry.First(x => x.key == key).text;
        }
        catch (InvalidOperationException)
        {
            Debug.LogError("InvalidOperationException : Key \"" + key + "\" not found for LangCode \"" + langCode + "\"");
            res = "<Error : Key Missing \"" + key + "\">";
        }
        return res;
    }

    /// <summary>
    /// Finds the text of the current language by using a specific key.
    /// Exemple: For the key "SCREEN2_TITLE" and the current lang "fr" it will return "REGLES DU JEU."
    /// For the key "SCREEN2_TITLE" and the current lang "en" it will return "GAME RULES."
    /// </summary>
    /// <param name="key">The key of the text.</param>
    /// <returns>The text translated to a specific language.</returns>
    public string GetText(string key)
    {
        return GetText(key, _currentLang.code);
    }

    private void OnDestroy()
    {
        s_instance = null;
    }
}
