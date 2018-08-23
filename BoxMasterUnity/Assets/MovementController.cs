using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public float rotationSpeed = 50.0f;

	public float zRotationSpeed = 20.0f;

	// Update is called once per frame
	void FixedUpdate () {
		transform.RotateAround (transform.position, Vector3.forward, zRotationSpeed * Time.fixedDeltaTime); 
		//transform.Rotate (Vector3.right * 60.0f * Time.fixedDeltaTime);
	}

	public void OnHit(Vector3 cameraForward, RaycastHit hit, Vector3 rotationVector) {
		GetComponent<Rigidbody> ().AddForceAtPosition (GameManager.instance.GetCamera(0).transform.forward * rotationSpeed, hit.point, ForceMode.Impulse);
	}
}
