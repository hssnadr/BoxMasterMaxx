using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILangMenu : MonoBehaviour {
	[SerializeField]
	protected UILangSelectButton _startLangButtonPrefab;
	[SerializeField]
	protected GameObject _copyrightButtonPrefab;
	[SerializeField]
	protected GameObject _soundButtonPrefab;
	[SerializeField]
	protected GameObject _separatorPrefab;

	void Start()
	{
		CreateButtons ();
	}

	void CreateButtons()
	{
		foreach (var buttonType in GameManager.instance.gameSettings.menuLayout) {
			switch (buttonType) {
			case ButtonType.Start:
				CreateLangButtons ();
				break;
			case ButtonType.Copyright:
				CreateCopyrightButton ();
				break;
			case ButtonType.Sound:
				CreateSoundButton ();
				break;
			case ButtonType.Separator:
				CreateSeparatorButton ();
				break;
			}
		}
	}

	void CreateLangButtons () {
		foreach (var lang in GameManager.instance.gameSettings.langAppEnable) {
			var langSelectButton = GameObject.Instantiate (_startLangButtonPrefab, this.transform);
			langSelectButton.lang = lang;
		}
	}

	void CreateCopyrightButton () {
		GameObject.Instantiate (_copyrightButtonPrefab, this.transform);
	}

	void CreateSoundButton () {
		GameObject.Instantiate (_soundButtonPrefab, this.transform);
	}

	void CreateSeparatorButton () {
		GameObject.Instantiate (_separatorPrefab, this.transform);
	}
}
