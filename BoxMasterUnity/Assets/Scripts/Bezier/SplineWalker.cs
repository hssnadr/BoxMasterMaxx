// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class SplineWalker : MonoBehaviour
    {
        public enum SplineWalkerMode
        {
            Once,
            Loop,
            PingPong
        }

        /// <summary>
        /// The spline on which the gameObject will walk.
        /// </summary>
        [Tooltip("The spline on which the gameObject will walk.")]
        public BezierSpline spline;
        /// <summary>
        /// The duration of the walk.
        /// </summary>
        [Tooltip("The duration of the walk.")]
        public float duration;
        /// <summary>
        /// Whether the gameObject look at the direction it's going.
        /// </summary>
        [Tooltip("Whether the gameObject look at the direction it's going.")]
        public bool lookForward;
        /// <summary>
        /// Whether the walk is forward or backward.
        /// </summary>
        [Tooltip("Whether the walk is forward or backward.")]
        public bool goingForward = true;
        /// <summary>
        /// The walking mode.
        /// Once = The gameObject walks along the path only once.
        /// Loop = The gameObject loops along the path.
        /// PingPong = The gameObject goes back and forth along the path.
        /// </summary>
        [Tooltip("The walking mode.\nOnce = The gameObject walks along the path only once.\nLoop = The gameObject loops along the path.\nPingPong = The gameObject goes back and forth along the path.")]
        public SplineWalkerMode mode;
        protected float _progress;

        protected virtual void Update()
        {
            if (goingForward)
            {
                _progress += Time.deltaTime / duration;
                if (_progress > 1.0f)
                {
                    if (mode == SplineWalkerMode.Once)
                        _progress = 1.0f;
                    else if (mode == SplineWalkerMode.Loop)
                        _progress -= 1.0f;
                    else
                    {
                        _progress = 2.0f - _progress;
                        goingForward = false;
                    }
                }
            }
            else
            {
                _progress -= Time.deltaTime / duration;
                if (_progress < 0.0f)
                {
                    _progress = -_progress;
                    goingForward = true;
                }
            }

            Vector3 position = spline.GetPoint(_progress);
            transform.position = position;
            if (lookForward)
                transform.LookAt(position + spline.GetDirection(_progress));
        }
    }
}
