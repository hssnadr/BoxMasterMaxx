// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTarget : MonoBehaviour
{
    public uint playerIndex;

    private float _time;

    public float timeUntilMove = 3.0f;

    private void OnEnable()
    {
        ImpactPointControl.onImpact += OnImpact;
    }

    private void OnDisable()
    {
        ImpactPointControl.onImpact -= OnImpact;
    }

    private void Start()
    {
        _time = Time.time;
    }

    private void OnImpact(Vector2 position)
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
            if (Input.GetMouseButtonDown(0))
                OnImpact(Input.mousePosition);
        }
    }
}
