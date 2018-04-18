using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPage : MonoBehaviour, IHideable {
	[SerializeField]
	private CanvasGroup _canvasGroup;

	void Awake () {
		_canvasGroup = GetComponent<CanvasGroup> ();
		Hide ();
	}

	public void Hide () {
		_canvasGroup.alpha = 0;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}

	public void Show ()
	{
		_canvasGroup.alpha = 1;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
	}
}
