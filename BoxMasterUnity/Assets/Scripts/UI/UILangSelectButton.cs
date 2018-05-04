using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILangSelectButton : MonoBehaviour {
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

	protected UIScreenMenu _UIPageMenu;

	void Start()
	{
		_UIPageMenu = GetComponentInParent<UIScreenMenu> ();

		if (_button == null)
			_button = GetComponentInChildren<Button> ();
		_button.onClick.AddListener (() => 
			{
				TextManager.instance.currentLang = lang;
				_UIPageMenu.GoToFirstPage();
			});
		
		_background.color = lang.color;

		if (_text != null) {
			_text.text = TextManager.instance.GetText (textKey, lang.code);
			_text.fontStyle = GameManager.instance.gameSettings.defaultLanguage.Equals(lang) ? FontStyle.Bold : FontStyle.Normal;
		}
		if (_highlightedText != null) {
			_highlightedText.text = TextManager.instance.GetText (textKey, lang.code);
			_highlightedText.fontStyle = GameManager.instance.gameSettings.defaultLanguage.Equals(lang) ? FontStyle.Bold : FontStyle.Normal;
			_highlightedText.color = lang.color;
		}
	}
}