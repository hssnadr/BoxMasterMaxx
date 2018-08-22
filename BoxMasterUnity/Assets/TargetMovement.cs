using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour {
	public RandomTarget[] _targets;

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

	private void Awake()
	{
		_targets = GetComponentsInChildren<RandomTarget> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.RotateAround (transform.position, Vector3.forward, 60.0f * Time.fixedDeltaTime); 
		//transform.Rotate (Vector3.right * 60.0f * Time.fixedDeltaTime);
	}

	public void Activate(bool activated)
	{
		int rand = Random.Range (0, _targets.Length);
		for (int i = 0; i < _targets.Length; i++) {
			_targets [i].activated = (i == rand && activated);
		}
	}

	public void OnHit(Vector3 cameraForward, RaycastHit hit) {
		Debug.Log ("hey");
		GetComponent<Rigidbody> ().AddRelativeTorque (50.0f, 0.0f, 0.0f, ForceMode.Impulse);
	}
}
