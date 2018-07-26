using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPoint : MonoBehaviour {
    public GameObject target;
    public Quaternion destinationRotation;
    public float timeToReachDestination;
    private float t = 0.0f;

    public void Update()
    {
        t += Time.deltaTime / timeToReachDestination;
        transform.position = Vector2.Lerp(transform.position, target.transform.position, t);
        transform.rotation = Quaternion.Lerp(this.transform.rotation, destinationRotation, t);
    }
}
