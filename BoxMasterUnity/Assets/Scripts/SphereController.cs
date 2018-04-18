using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SphereController : MonoBehaviour {
	[SerializeField]
	private Rigidbody _rigidbody;
	[SerializeField]
	private float _startingSpeed = 5.0f;

	void Start()
	{
		_rigidbody = GetComponent<Rigidbody> ();
		_rigidbody.AddForce (Vector3.forward * _startingSpeed, ForceMode.Impulse);
	}

	void OnCollisionEnter(Collision col)
	{
	}
}