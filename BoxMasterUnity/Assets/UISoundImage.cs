using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISoundImage : MonoBehaviour {
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
