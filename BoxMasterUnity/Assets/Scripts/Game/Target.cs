// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Serial;
using System;

namespace CRI.HitBox.Game
{
    public class Target : MonoBehaviour
    {
        /// <summary>
        /// The index of the only player who can hit this target.
        /// </summary>
        internal int playerIndex;
        /// <summary>
        /// Is the target activated?
        /// </summary>
        [SerializeField]
        [Tooltip("Is the target activated?")]
        internal bool activated = false;
        /// <summary>
        /// Material when the target is activated.
        /// </summary>
        [SerializeField]
        [Tooltip("Material when the target is activated.")]
        public Material _activatedMaterial = null;
        /// <summary>
        /// Material when the target is deactivated.
        /// </summary>
        [SerializeField]
        [Tooltip("Material when the target is deactivated.")]
        public Material _deactivatedMaterial = null;

        /// <summary>
        /// Time of the last hit.
        /// </summary>
        public float lastHit
        {
            get;
            private set;
        }

        /// <summary>
        /// Z position of the target during the previous frame.
        /// </summary>
        public float zPosition
        {
            get; private set;
        }

        internal void Hit()
        {
            activated = false;
            lastHit = Time.time;
        }
        
        private void Update()
        {
            GetComponent<Renderer>().material = activated ? _activatedMaterial : _deactivatedMaterial;
            if (GameManager.instance.GetCamera(playerIndex).GetComponent<Camera>().orthographic)
                transform.rotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
            else
            {
                transform.LookAt(GameManager.instance.GetCamera(playerIndex).transform.position);
                transform.Rotate(90.0f, 0.0f, 0.0f);
            }
        }

        private void LateUpdate()
        {
            zPosition = transform.position.z;
        }
    }
}