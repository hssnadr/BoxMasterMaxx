// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTarget : MonoBehaviour
{
    public delegate void RandomTargetEvent(int playerIndex);
    public static event RandomTargetEvent onHit;

    private float _time;

    public float timeUntilMove = 3.0f;

    public int playerIndex;

    public float rotationSpeed = 2.0f;

	public bool activated = false;

	public Color activatedColor = Color.green;

	public Color deactivatedColor = Color.red;

	public Vector3 rotationVector;

    private float _zPosition;

    private void OnEnable()
    {
        ImpactPointControl.onImpact += OnImpact;
        GameManager.onGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        ImpactPointControl.onImpact -= OnImpact;
        GameManager.onGameEnd -= OnGameEnd;
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        Debug.LogWarning("ON IMPACT");
        if (playerIndex == this.playerIndex)
            ScoreUp(position);
    }

    private void ScoreUp(Vector2 position)
    {
		int layerMask = 1 << (8 + playerIndex) | 1 << 10;
		Vector3 cameraForward = GameManager.instance.GetCamera (playerIndex).transform.forward;
		RaycastHit hit;
		Debug.DrawRay (position, cameraForward * 5000.0f, Color.yellow, 10.0f);
		if (Physics.Raycast (position, cameraForward, out hit, Mathf.Infinity, layerMask) && hit.collider.gameObject == this.gameObject)
		{
			if (activated) {
				GameManager.instance.ScoreUp ();
				Debug.LogWarning ("worked");
				_time = Time.time;
				onHit (playerIndex);
                activated = false;
            } else {
				GameManager.instance.Miss ();
			}
            bool direction = (transform.position.z - _zPosition) * cameraForward.z >= 0;
            Debug.LogWarning(direction);
            GetComponentInParent<MovementController> ().OnHit (direction ? cameraForward : -cameraForward, hit, rotationVector);
		} else {
			GameManager.instance.Miss ();
		}
		/*
        var camera = GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>();
        var rect = GetComponent<RectTransform>().rect;
        rect.position = GetComponent<RectTransform>().position;
        Vector2 size = Vector2.Scale(rect.size, transform.lossyScale);
        var newRect = new Rect(rect.xMin - size.x / 2, rect.yMin - size.y / 2, size.x, size.y);
		var secRect = new Rect (position.x - size.x / 2.0f, position.y - size.y / 2.0f, size.x, size.y);
        Debug.Log(newRect);
        Debug.Log(position);

		if (newRect.Overlaps(secRect, true) && activated)
        {
            GameManager.instance.ScoreUp();
            Debug.LogWarning("worked");
            _time = Time.time;
            //RandomPosition();
            onHit(playerIndex);
            //Destroy(gameObject);
        }
        else
        {
            GameManager.instance.Miss();
        }
		*/
    }

    private void OnMouseDown()
    {
        if (GetComponentInParent<MovementController>().mousePlayerIndex == playerIndex)
        {
            Debug.Log("On Mouse Down");
            ScoreUp(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));
        }
    }

    protected virtual void RandomPosition()
    {
        var bounds = GameManager.instance.GetCamera(playerIndex).bounds;
        this.transform.position = new Vector3(
            Random.Range(
                bounds.min.x + GetComponent<RectTransform>().rect.width / 2.0f,
                bounds.max.x - (GetComponent<RectTransform>().rect.height / 2.0f)
            ),
            Random.Range(
                bounds.min.y + GetComponent<RectTransform>().rect.height / 2.0f,
                bounds.max.y - (GetComponent<RectTransform>().rect.height / 2.0f)
            ),
            0.0f
        );
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, 0.0f);

    }

    protected virtual void Update()
    {
        /*if (GameManager.instance.gameHasStarted)
        {
            if (_time + timeUntilMove <= Time.time)
            {
                _time = Time.time;
                RandomPosition();
            }
        }*/
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onHit(playerIndex);
            Destroy(gameObject);
        }

        var angles = this.transform.rotation.eulerAngles;
        //this.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z + rotationSpeed);
		GetComponent<MeshRenderer>().material.SetColor("_Color", activated ? activatedColor : deactivatedColor);
        _zPosition = transform.position.z;
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
