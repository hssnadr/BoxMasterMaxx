// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UITimeoutWarning : MonoBehaviour
    {
        /// <summary>
        /// Canvas of the text of the first timeout warning.
        /// </summary>
        [SerializeField]
        [Tooltip("Canvas of the text of the first timeout warning.")]
        private CanvasGroup _canvas1 = null;
        /// <summary>
        /// Canvas of the text of the second timeout warning.
        /// </summary>
        [SerializeField]
        [Tooltip("Canvas of the text of the second timeout warning.")]
        private CanvasGroup _canvas2 = null;

        private void Update()
        {
            bool setup = (ApplicationManager.instance.setupPhase);
            _canvas1.alpha = setup ? 0.0f : 1.0f;
            _canvas2.alpha = setup ? 1.0f : 0.0f;
        }
    }
}