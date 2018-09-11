// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRI.HitBox.UI
{
    public class UISoundButton : MonoBehaviour
    {
        [SerializeField]
        protected Slider _soundSlider;
        [SerializeField]
        protected UISoundImage _soundImage;

        private void Start()
        {
            if (_soundSlider == null)
                _soundSlider = GetComponentInChildren<Slider>();
            if (_soundImage == null)
                _soundImage = GetComponentInChildren<UISoundImage>();

            _soundSlider.value = AudioManager.instance.volume;
            _soundImage.SetTexture(_soundSlider.value != 0);

            _soundSlider.onValueChanged.AddListener(delegate
            {
                OnValueChanged();
            });
        }

        private void OnValueChanged()
        {
            _soundImage.SetTexture(_soundSlider.value != 0);
            AudioManager.instance.volume = _soundSlider.value;
        }
    }
}