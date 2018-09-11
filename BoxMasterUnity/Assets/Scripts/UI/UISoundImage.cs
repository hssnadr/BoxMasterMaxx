// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UISoundImage : MonoBehaviour
    {
        [SerializeField]
        protected Sprite offSprite;
        [SerializeField]
        protected Sprite onSprite;

        public void SetTexture(bool isOn)
        {
            if (isOn)
                GetComponent<Image>().overrideSprite = onSprite;
            else
                GetComponent<Image>().overrideSprite = offSprite;
        }
    }
}