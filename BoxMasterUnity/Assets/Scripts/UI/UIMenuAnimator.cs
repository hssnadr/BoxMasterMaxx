using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CanvasGroup))]
public class UIMenuAnimator : MonoBehaviour, IHideable {
	[SerializeField]
	protected Animator _animator;
	[SerializeField]
	protected bool _open = false;
	[SerializeField]
	protected CanvasGroup _canvasGroup;

	protected Coroutine _closeMenuRoutine = null;

	void OnEnable()
	{
		GameManager.onActivity += OnActivity;
	}

	void OnActivity ()
	{
		if (_open)
			CloseMenuOnCooldown ();
	}

	void Start()
	{
		_animator = GetComponent<Animator> ();
		_canvasGroup = GetComponent<CanvasGroup> ();
	}

	public void ToggleAnimation() {
		SetState (!_open);
	}

	#region IHideable implementation

	public void Hide ()
	{
		_canvasGroup.alpha = 0;
		_canvasGroup.interactable = false;
		_canvasGroup.blocksRaycasts = false;
		StopCoroutine (_closeMenuRoutine);
		SetState (false);
	}

	public void Show ()
	{
		_canvasGroup.alpha = 1;
		_canvasGroup.interactable = true;
		_canvasGroup.blocksRaycasts = true;
	}

	#endregion

	public void SetState (bool state)
	{
		_open = state;
		_animator.SetBool ("Open", state);
		if (_open)
			CloseMenuOnCooldown ();
	}

	public void CloseMenuOnCooldown()
	{
		if (_closeMenuRoutine != null)
			StopCoroutine (_closeMenuRoutine);
		_closeMenuRoutine = StartCoroutine (CloseMenuRoutine());
	}

	IEnumerator CloseMenuRoutine() {
		yield return new WaitForSeconds (GameManager.instance.gameSettings.timeOutMenu);
		if (_open)
			SetState (false);
	}
}
