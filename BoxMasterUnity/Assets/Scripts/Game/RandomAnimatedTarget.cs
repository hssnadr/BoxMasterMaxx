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

    public AnimatedPoint line1;
    public AnimatedPoint line2;
    public AnimatedFadeIn target;

    protected override void RandomPosition()
    {
        var bounds = GameManager.instance.GetCamera(playerIndex).bounds;
        _start = this.transform.position;
        
        
       target.transform.position =
            new Vector3(
            Random.Range(
                bounds.min.x + target.GetComponent<RectTransform>().rect.height / 10.0f,
                bounds.max.x - (target.GetComponent<RectTransform>().rect.height / 10.0f)
            ),
            Random.Range(
                bounds.min.y + target.GetComponent<RectTransform>().rect.width / 10.0f,
                bounds.max.y - (target.GetComponent<RectTransform>().rect.width / 10.0f)
            ),
            0.0f
        );
        target.transform.localPosition = new Vector3(target.transform.localPosition.x, target.transform.localPosition.y, 0.0f);

        float x = Random.Range(minDist, maxDist);
        float y = Random.Range(minDist, maxDist);

        line1.transform.position = new Vector3(target.transform.position.x + x, target.transform.position.y + y, 0.0f);
        line1.destination = target.transform.position;

        float x2 = Random.Range(minDist, maxDist);
        float y2 = Random.Range(minDist, maxDist);

        line2.transform.position = new Vector3(target.transform.position.x + x2, target.transform.position.y + y2, 0.0f);
        line2.destination = target.transform.position;

        float random = Random.Range(0.0f, 360.0f);
        int sign = 2 * Random.Range(0, 2) - 1;
        target.transform.rotation = Quaternion.Euler(0.0f, 0.0f, random - 45.0f);
        line1.destinationRotation = Quaternion.Euler(0.0f, 0.0f, random - 45.0f * sign);
        line1.transform.rotation = Quaternion.Euler(0.0f, 0.0f, random + Random.Range(minRotation, maxRotation) * sign);
        line2.destinationRotation = Quaternion.Euler(0.0f, 0.0f, random - 45.0f * -sign);
        line2.transform.rotation = Quaternion.Euler(0.0f, 0.0f, random + Random.Range(minRotation, maxRotation) * -sign); 
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}
