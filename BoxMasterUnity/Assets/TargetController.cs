using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
	private int _playerIndex;

	public int playerIndex {
		get { return _playerIndex; }
		set {
			foreach (var target in _targets)
				target.playerIndex = value;
			foreach (Transform child in transform)
				child.gameObject.layer = 8 + value;
			_playerIndex = value;
			gameObject.layer = 8 + value;
		}
	}

	private RandomTarget[] _targets;

	private void Awake()
	{
		_targets = GetComponentsInChildren<RandomTarget> ();
	}


	public void Activate(int takeCount = 1)
	{
		var random = new System.Random ();
		RandomTarget[] targetsToActivate = _targets.Where (x => !x.activated).OrderBy (i => random.Next ()).Take (takeCount).ToArray ();
		foreach (var target in targetsToActivate)
			target.activated = true;
	}
}
