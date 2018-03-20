using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIMenuAnimator : MonoBehaviour {
	[SerializeField]
	protected Animator _animator;
	[SerializeField]
	protected bool _open = false;

	void Start()
	{
		_animator = GetComponent<Animator> ();
	}

	public void ToggleAnimation() {
		SetState (!_open);
	}

	public void SetState (bool state)
	{
		_open = state;
		_animator.SetBool ("Open", state);
	}
}
