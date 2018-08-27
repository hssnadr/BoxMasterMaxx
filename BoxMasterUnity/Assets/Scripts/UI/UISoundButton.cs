using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundButton : MonoBehaviour {
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
