using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TranslatedText : MonoBehaviour {
	/// <summary>
	/// The key to the text to translate.
	/// </summary>
	[Tooltip("The key to the text to translate.")]
	public string textKey;

	[SerializeField]
	private Text _text;

	void OnEnable() {
		TextManager.onLangChange += OnLangChange;
	}

	void OnLangChange (LangApp lang)
	{
		SetText ();
	}

	void SetText() {
		_text.text = TextManager.instance.GetText (textKey);
	}
	
	void Start() {
		_text = GetComponent<Text> ();
		SetText ();
	}

	void OnDisable() {
		TextManager.onLangChange -= OnLangChange;
	}
}
