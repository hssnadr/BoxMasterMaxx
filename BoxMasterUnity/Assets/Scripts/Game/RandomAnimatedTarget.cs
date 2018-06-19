// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimatedTarget : RandomTarget
{
    public EasingFunction.Ease ease;
    [RangeAttribute(0.0f, 1.0f)]
    public float damping = 0.63f;

    public float minDist = 0.0f;
    public float maxDist = 10.0f;

    public Vector2 _start;
    public Vector2 _destination;
    public Quaternion _destinationRotation;
    public float minRotation;
    public float maxRotation;
    private float _currentDistance;
    private float _distance;
    private Vector2 _direction;

    protected override void RandomPosition()
    {
        base.RandomPosition();
        var bounds = GameManager.instance.GetCamera(playerIndex).bounds;
        _start = this.transform.position;

        float x = Random.Range(minDist, maxDist);
        float y = Random.Range(minDist, maxDist - x);
        _destination = new Vector2(
            Mathf.Clamp(this.transform.position.x + x,
            bounds.min.x + GetComponent<RectTransform>().rect.width / 2.0f,
            bounds.max.x - GetComponent<RectTransform>().rect.height),
            Mathf.Clamp(this.transform.position.y + y,
            bounds.min.y + GetComponent<RectTransform>().rect.height / 2.0f,
            bounds.max.y - GetComponent<RectTransform>().rect.height / 2.0f)
            );

        float random = Random.Range(0.0f, 360.0f);
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, random);
        _destinationRotation = Quaternion.Euler(0.0f, 0.0f, random + Random.Range(minRotation, maxRotation) * (2 * Random.Range(0, 2) - 1));
        _currentDistance = 0.0f;
        _distance = Vector2.Distance(_destination, _start);
        _direction = (_destination - _start).normalized;
    }

    protected override void Update()
    {
        base.Update();
        _currentDistance = EasingFunction.GetEasingFunction(ease)(_currentDistance, _distance, damping * Time.deltaTime);
        this.transform.position = _start + _currentDistance * _direction;
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, _destinationRotation, 0.67f * Time.deltaTime);
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
