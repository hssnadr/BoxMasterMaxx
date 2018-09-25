// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBox
{
    public class DisplayManager : MonoBehaviour
    {
        /// <summary>
        /// The camera for the menu.
        /// </summary>
        [SerializeField]
        [Tooltip("The camera for the menu.")]
        private Camera _menuCamera = null;
        /// <summary>
        /// The camera for the big screen.
        /// </summary>
        [SerializeField]
        [Tooltip("The camera for the big screen.")]
        private Camera _bigScreenCamera = null;
        private void Start()
        {
#if !UNITY_EDITOR
        Debug.Log("Displays connected: " + Display.displays.Length);
        if (Display.displays.Length > 1)
        {
            Display.displays[1].Activate();
        }
#endif
        }

        public void SwapDisplays()
        {
            int temp = _menuCamera.targetDisplay;
            _menuCamera.targetDisplay = _bigScreenCamera.targetDisplay;
            _bigScreenCamera.targetDisplay = temp;
        }
    }
}