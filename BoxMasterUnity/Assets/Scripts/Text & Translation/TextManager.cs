// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class TextManager : MonoBehaviour
{
    public delegate void TextManagerLangHandler(LangApp lang);
    public static event TextManagerLangHandler onLangChange;

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

    private static TextManager s_instance = null;

    public const string text_lang_path_base = "lang/[lang_app]/text/text.xml";

    public const string text_lang_common_path = "lang/Common/text/text.xml";

    private LangApp _currentLang;

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

    public List<LangText> langTextList = new List<LangText>();

    private void Awake()
    {
        Init();
        foreach (var langEnable in GameManager.instance.gameSettings.langAppEnable)
        {
            var langText = LoadLangText(langEnable.code);
            langTextList.Add(langText);
        }
        LoadCommonLangText();
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

    public void SetDefaultLang()
    {
        currentLang = GameManager.instance.gameSettings.defaultLanguage;
    }

    public string GetLangTextPath(string langCode)
    {
        var path = text_lang_path_base.Replace("[lang_app]", langCode);
        return path;
    }

    public LangText LoadCommonLangText()
    {
        return LangText.Load(Path.Combine(Application.dataPath, text_lang_common_path));
    }

    public LangText LoadLangText(string langCode)
    {
        return LangText.Load(Path.Combine(Application.dataPath, GetLangTextPath(langCode)));
    }

    public string GetText(string key, string langCode)
    {
        return langTextList.First(x => x.code == langCode).arrayOfLangTextEntry.First(x => x.key == key).text;
    }

    public string GetText(string key)
    {
        return GetText(key, _currentLang.code);
    }

    private void OnDestroy()
    {
        s_instance = null;
    }
}
