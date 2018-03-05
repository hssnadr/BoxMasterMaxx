using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
	public static TextManager instance {
		get {
			if (_instance == null) {
				new GameObject ("TextManager").AddComponent<TextManager> ().Init ();
			}
			return _instance;
		}
	}

	private static TextManager _instance = null;

	public List<LangText> langText = new List<LangText> ();

	void Awake() {
		Init ();
	}

	void Init()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (_instance != this)
			Destroy (gameObject);
	}

	public static string GetText (string key)
	{
		return "";
	}

	void OnDestroy() {
		_instance = null;
	}
}
