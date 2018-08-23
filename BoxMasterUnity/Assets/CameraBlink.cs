using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlink : MonoBehaviour {
	public Color color1;
	public Color color2;
	public float time = 0.5f;

	private void Start()
	{
		StartCoroutine (Blink ());
	}

	IEnumerator Blink()
	{
		while (true) {
			if (GetComponent<Camera> ().backgroundColor == color1)
				GetComponent<Camera> ().backgroundColor = color2;
			else
				GetComponent<Camera> ().backgroundColor = color1;
			yield return new WaitForSeconds (0.5f);
		}
	}
}
