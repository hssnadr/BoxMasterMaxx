// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    private float _time;

    public float timeUntilMove = 3.0f;

    public int playerIndex;

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
        _time = Time.time + Random.Range(0, timeUntilMove);
    }

    private void OnImpact(Vector2 position, int playerIndex)
    {
        if (playerIndex == this.playerIndex)
            ScoreUp(position);
    }

    private void ScoreUp(Vector2 position)
    {
        var rect = this.GetComponent<RectTransform>().rect;
        rect.position = GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().WorldToScreenPoint(GetComponent<RectTransform>().position);
        var screenPosition = GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().WorldToScreenPoint(position);
        if (rect.Contains(screenPosition))
        {
            GameManager.instance.ScoreUp(playerIndex);
            Debug.Log("worked");
            _time = Time.time;
            var bounds = GameManager.instance.GetCamera(playerIndex).bounds;
            GetComponent<RectTransform>().position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        }
        Debug.Log("Screen Position: " + screenPosition);
        Debug.Log("Rect: " + rect);
    }

    private void OnMouseDown()
    {
        ScoreUp(Input.mousePosition);
    }

    private void Update()
    {
        if (GameManager.instance.gameHasStarted)
        {
            if (_time + timeUntilMove <= Time.time)
            {
                _time = Time.time;
                var bounds = GameManager.instance.GetCamera(playerIndex).bounds;
                GetComponent<RectTransform>().position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
            }
        }
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
