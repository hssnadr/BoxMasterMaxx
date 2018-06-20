using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPoint : MonoBehaviour {
    public Vector2 destination;
    public Quaternion destinationRotation;
    public float timeToReachDestination;
    private float t = 0.0f;

    public void Update()
    {
        t += Time.deltaTime / timeToReachDestination;
        transform.position = Vector2.Lerp(transform.position, destination, t);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, destinationRotation, t);
    }
}
