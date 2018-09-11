// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UITimeOutScreen : UIScreen
    {
        [SerializeField]
        protected Slider _slider;

        protected override void Awake()
        {
            base.Awake();
            if (_slider == null)
                _slider = GetComponentInChildren<Slider>();
        }

        protected override void Start()
        {
            _slider.minValue = 0;
            _slider.maxValue = GameManager.instance.menuSettings.timeOut;
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
            _slider.value = GameManager.instance.timeOut2;
        }
    }
}