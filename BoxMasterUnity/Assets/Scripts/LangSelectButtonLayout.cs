using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangSelectButtonLayout : MonoBehaviour {
	[SerializeField]
	protected LangSelectButton _langSelectButtonPrefab;

	void Start()
	{
		foreach (var lang in GameManager.instance.gameSettings.langAppEnable) {
			var langSelectButton = GameObject.Instantiate (_langSelectButtonPrefab, this.transform);
			langSelectButton.lang = lang;
		}
	}
}
