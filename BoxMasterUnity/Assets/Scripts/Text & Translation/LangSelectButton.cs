using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LangSelectButton : MonoBehaviour {
	public LangApp lang;

	public string textKey;

	[SerializeField]
	protected Button _button;
	[SerializeField]
	protected Text _buttonText;
	[SerializeField]
	protected UIPageMenu _UIPageMenu;

	protected void Start()
	{
		_UIPageMenu = GetComponentInParent<UIPageMenu> ();
		if (_button != null)
			_button = GetComponent<Button> ();
		_button.onClick.AddListener (() => 
			{
				TextManager.instance.currentLang = lang;
				_UIPageMenu.GoToFirstPage();
			});

		if (_buttonText != null)
			_buttonText.text = TextManager.instance.GetText (textKey, lang.code);
	}
}
