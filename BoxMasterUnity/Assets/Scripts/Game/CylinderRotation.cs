using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderRotation : MonoBehaviour {
    /// <summary>
    /// The speed of the rotation.
    /// </summary>
    [SerializeField]
    protected float _speed;

    public float speed
    {
        get { return _speed; }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
