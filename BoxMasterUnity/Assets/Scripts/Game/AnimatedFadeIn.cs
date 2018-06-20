using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedFadeIn : MonoBehaviour {
    public float timeToReachDestination;
    private float t = 0.0f;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachDestination;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, t);
    }
}
