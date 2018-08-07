// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    public delegate void RandomTargetEvent(int playerIndex);
    public static event RandomTargetEvent onHit;

    private float _time;

    public float timeUntilMove = 3.0f;

    public int playerIndex;

    public float rotationSpeed = 2.0f;

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

    private void Start()
    {
        RandomPosition();
        _time = Time.time + Random.Range(0, timeUntilMove);
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        Debug.LogWarning("ON IMPACT");
        if (playerIndex == this.playerIndex)
            ScoreUp(position);
    }

    private void ScoreUp(Vector2 position)
    {
        var camera = GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>();
        var rect = GetComponent<RectTransform>().rect;
        rect.position = GetComponent<RectTransform>().position;
        Vector2 size = Vector2.Scale(rect.size, transform.lossyScale);
        var newRect = new Rect(rect.xMin - size.x / 2, rect.yMin - size.y / 2, size.x, size.y);
        Debug.Log(newRect);
        Debug.Log(position);

        if (newRect.Contains(position, true))
        {
            GameManager.instance.ScoreUp();
            Debug.LogWarning("worked");
            _time = Time.time;
            RandomPosition();
            onHit(playerIndex);
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("On Mouse Down");
        ScoreUp(GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));
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
        if (GameManager.instance.gameHasStarted)
        {
            if (_time + timeUntilMove <= Time.time)
            {
                _time = Time.time;
                RandomPosition();
            }
        }
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
        this.transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z + rotationSpeed);
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
