using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIQuitButton : MonoBehaviour {
	void Start()
	{
		GetComponent<Button> ().onClick.AddListener (() => {
			GetComponentInParent<UIScreenMenu> ().GoToHome ();
		}
		);
	}
}
