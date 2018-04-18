using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class TextManager : MonoBehaviour {
	public delegate void TextManagerLangHandler (LangApp lang);
	public static event TextManagerLangHandler onLangChange;

	public static TextManager instance {
		get {
			if (_instance == null) {
				new GameObject ("TextManager").AddComponent<TextManager> ().Init ();
			}
			return _instance;
		}
	}

	private static TextManager _instance = null;

	public const string text_lang_path_base = "lang/[lang_app]/text/text.xml";

	public const string text_lang_common_path = "lang/Common/text/text.xml";

	private LangApp _currentLang;

	public LangApp currentLang {
		get {
			return _currentLang;
		}
		set {
			_currentLang = value;
			if (onLangChange != null)
				onLangChange (_currentLang);
		}
	}

	public List<LangText> langTextList = new List<LangText>();
		
	void Awake() {
		Init ();
		foreach (var langEnable in GameManager.instance.gameSettings.langAppEnable) {
			var langText = LoadLangText (langEnable.code);
			langTextList.Add (langText);
		}
		LoadCommonLangText ();
		_currentLang = GameManager.instance.gameSettings.defaultLanguage;
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (_instance != this)
			Destroy (gameObject);
	}

	public void SetDefaultLang()
	{
		currentLang = GameManager.instance.gameSettings.defaultLanguage;
	}

	public string GetLangTextPath (string langCode)
	{
		var path = text_lang_path_base.Replace ("[lang_app]", langCode);
		return path;
	}

	public LangText LoadCommonLangText ()
	{
		return LangText.Load (Path.Combine (Application.dataPath, text_lang_common_path));
	}

	public LangText LoadLangText (string langCode)
	{
		return LangText.Load (Path.Combine(Application.dataPath, GetLangTextPath(langCode)));
	}

	public string GetText (string key, string langCode)
	{
		return langTextList.First (x => x.code == langCode).arrayOfLangTextEntry.First (x => x.key == key).text;
	}

	public string GetText (string key)
	{
		return GetText (key, _currentLang.code);
	}

	void OnDestroy() {
		_instance = null;
	}
}
