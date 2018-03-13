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

	public LangApp _currentLang;

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
		foreach (var langAvailable in GameManager.instance.gameSettings.langAppAvailable) {
			var nlt = new LangText (langAvailable.code);
			nlt.Save (GetLangTextPath(langAvailable.code));
		}
		foreach (var langEnable in GameManager.instance.gameSettings.langAppEnable) {
			var langText = LoadLangText (langEnable.code);
			langTextList.Add (langText);
		}
		currentLang = GameManager.instance.gameSettings.defaultLanguage;
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (_instance != this)
			Destroy (gameObject);
	}

	public string GetLangTextPath (string langCode)
	{
		var path = text_lang_path_base.Replace ("[lang_app]", langCode);
		return path;
	}

	public LangText LoadLangText (string langCode)
	{
		return LangText.Load (Path.Combine(Application.dataPath, GetLangTextPath(langCode)));
	}

	public string GetText (string key)
	{
		return langTextList.First (x => x.code == currentLang.code).arrayOfLangTextEntry.First (x => x.key == key).text;
	}

	void OnDestroy() {
		_instance = null;
	}
}
