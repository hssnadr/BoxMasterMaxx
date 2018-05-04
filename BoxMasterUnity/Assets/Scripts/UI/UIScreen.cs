using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour, IHideable {
	[SerializeField]
	private CanvasGroup _canvasGroup;

	protected virtual void Start () {
		if (_canvasGroup == null)
			_canvasGroup = GetComponent<CanvasGroup> ();
		Hide ();
	}

	public void Hide () {
		Debug.Log ("Hide " + gameObject.name);
		_canvasGroup.alpha = 0;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
	}

	public void Show ()
	{
		Debug.Log ("Show " + gameObject.name);
		_canvasGroup.alpha = 1;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
	}

	protected virtual void Update()
	{
	}
}
