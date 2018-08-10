// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour
{
    private Rigidbody _rb;

    // Use this for initialization
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            setForce(new Vector3(0, 0, 500), ray.origin);
        }
    }

    public void setForce(Vector3 force, Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.forward, out hit))
        {
            if (hit.collider != null && hit.transform.tag == "target")
            {
                _rb.AddForceAtPosition(force, position);
            }
        }
    }
}
