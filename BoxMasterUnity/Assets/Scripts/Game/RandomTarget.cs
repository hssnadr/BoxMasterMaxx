// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Arduino;
using System;

namespace CRI.HitBox.Game
{
    public class RandomTarget : MonoBehaviour
    {
        public int playerIndex;

        public bool activated = false;

        public Color activatedColor = Color.green;

        public Color deactivatedColor = Color.red;

        public float lastHit
        {
            get;
            private set;
        }

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
            GetComponent<MeshRenderer>().material.SetColor("_Color", activated ? activatedColor : deactivatedColor);
            zPosition = transform.position.z;
        }
    }
}