// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using HitBox.Arduino;

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
    }

    private void OnDisable()
    {
        ImpactPointControl.onImpact -= OnImpact;
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        if (playerIndex == this.playerIndex)
            ScoreUp(position);
    }

    private void ScoreUp(Vector2 position)
    {
		int layerMask = 1 << (8 + playerIndex) | 1 << 10;
		Vector3 cameraForward = GameManager.instance.GetCamera (playerIndex).transform.forward;
		Debug.DrawRay (position, cameraForward * 5000.0f, Color.yellow, 10.0f);
        var hits = Physics.RaycastAll(position, cameraForward, Mathf.Infinity, layerMask);
        if (hits.Any(x => x.collider.gameObject == gameObject))
        {
            RaycastHit hit = hits.First(x => x.collider.gameObject == gameObject);
            if (activated)
            {
                Vector2 center = this.GetComponent<Collider>().bounds.center;
                Vector2 hitPoint = hit.point;
                Debug.Log("center = " + center);
                Debug.Log("point = " + hitPoint);
                float distance = Vector2.Distance(hitPoint, center);
                Debug.Log("distance = " + distance);
                int min = GameManager.instance.gameplaySettings.minPoints;
                int max = GameManager.instance.gameplaySettings.maxPoints;
                float maxDistance = this.GetComponent<Collider>().bounds.extents.x;
                float tolerance = GameManager.instance.gameplaySettings.tolerance;
                float minDistance = maxDistance * tolerance;
                int score = (int)Mathf.Clamp(max * (maxDistance - distance) / (maxDistance - minDistance), min, max);
                Debug.Log(max * (maxDistance - distance) / (maxDistance - minDistance));
                Debug.Log(score);
                GameManager.instance.ScoreUp(score);
                _time = Time.time;
                onHit(playerIndex);
                activated = false;
            }
            bool direction = (transform.position.z - _zPosition) * cameraForward.z >= 0;
            GetComponentInParent<MovementController>().OnHit(direction ? cameraForward : -cameraForward, hit, rotationVector);
        }
    }

#if UNITY_EDITOR
    private void OnMouseDown()
    {
        if (GetComponentInParent<MovementController>().mousePlayerIndex == playerIndex)
        {
            Vector3 mousePosition = Input.mousePosition;
            if (!GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().orthographic)
                mousePosition.z = transform.position.z;
            ScoreUp(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(mousePosition));
        }
    }
#endif

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
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onHit(playerIndex);
            Destroy(gameObject);
        }
#endif

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
