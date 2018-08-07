using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceTest : MonoBehaviour {
    private Rigidbody _rigidbody;

    public float force = 50;

    public int playerIndex;

    void OnEnable()
    {
        ImpactPointControl.onImpact += OnImpact;
    }

    void OnDisable()
    {
        ImpactPointControl.onImpact -= OnImpact;
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        Debug.Log(position);
        Debug.Log(GameManager.instance.GetCamera(playerIndex).transform.TransformDirection(Vector3.forward));
        Debug.DrawRay(position, GameManager.instance.GetCamera(playerIndex).transform.TransformDirection(Vector3.forward), Color.red, 15.0f);
        //if (Physics.Raycast(position, GameManager.instance.GetCamera(playerIndex).transform.TransformDirection(Vector3.forward), out hit))
        //{
            _rigidbody.AddForceAtPosition(
                GameManager.instance.GetCamera(playerIndex).transform.TransformDirection(Vector3.forward) * force,
                position,
                ForceMode.Impulse
                );
        //}
    }

    // Use this for initialization
    void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}

    
    // Update is called once per frame
    void OnMouseDown() {
        OnImpact(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition), playerIndex);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            playerIndex = 0;
        if (Input.GetKeyDown(KeyCode.Z))
            playerIndex = 1;
    }
}
