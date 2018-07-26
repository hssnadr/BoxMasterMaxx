// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Bounds bounds { get { return CameraBounds(); } }

   

    /// <summary>
    /// The ratio of the camera bound. At 1, the bound is the camera screen. At 0.5, it is 2 times smaller than the camera screen.
    /// </summary>
    [Tooltip("The ratio of the camera bound. At 1, the bound is the camera screen. At 0.5, it is 2 times smaller than the camera screen.")]
    [Range(0.0f, 1.0f)]
    public float ratio = 1.0f;

    /// <summary>
    /// Returns the bounds of the camera view
    /// </summary>
    /// <returns>The bounds.</returns>
    private Bounds CameraBounds()
    {
        int screenWidth = 0;
        int screenHeight = 0;
        try
        {
            if (GetComponent<Camera>().targetTexture != null)
            {
                screenWidth = GetComponent<Camera>().targetTexture.width;
                screenHeight = GetComponent<Camera>().targetTexture.height;
            }
            else
            {
                screenWidth = Display.displays[GetComponent<Camera>().targetDisplay].systemWidth;
                screenHeight = Display.displays[GetComponent<Camera>().targetDisplay].systemHeight;
            }
        }
        catch (IndexOutOfRangeException)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
        }

        float screenAspect = (float)screenWidth / (float)screenHeight;
        float cameraHeight = GetComponent<Camera>().orthographicSize * 2.0f;
        var bounds = new Bounds(
                        (Vector2)GetComponent<Camera>().transform.position,
                        new Vector3(cameraHeight * screenAspect, cameraHeight, 0.0f) * ratio
                    );
        return bounds;
    }
}