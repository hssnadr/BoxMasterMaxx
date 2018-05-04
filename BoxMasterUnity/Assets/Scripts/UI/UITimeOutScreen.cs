using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeOutScreen : UIScreen {
	[SerializeField]
	protected Slider _slider;

	protected override void Start ()
	{
		base.Start ();
		if (_slider == null)
			_slider = GetComponentInChildren<Slider> ();
		_slider.minValue = 0;
		_slider.maxValue = GameManager.instance.gameSettings.timeOut;
	}

	protected override void Update ()
	{
		base.Update ();
		_slider.value = GameManager.instance.timeOut2;
	}
}
