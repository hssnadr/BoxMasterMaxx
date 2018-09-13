using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PrecisionTest2: MonoBehaviour
    {
        public Slider slider;
        [Range(0, 1)]
        public float value = 1;

        public Transform fill;

        public List<Transform> stars = new List<Transform>();

        public float previousValue;

        void Start()
        {
            foreach (Transform child in fill)
            {
                stars.Add(child);
            }
        }

        void Update()
        {
                float newValue = GetRating(value);
                if (previousValue != newValue)
                {
                    foreach (var star in stars)
                        star.SetParent(null);
                }
                slider.value = newValue;
                if (previousValue != newValue)
                {
                    foreach (var star in stars)
                        star.SetParent(fill);
                }
                previousValue = newValue;
        }

        public float GetRating(float value)
        {
            int iValue = (int)(value * 100);
            float res = 0.0f;
            int starWidth = (int)stars[0].GetComponent<RectTransform>().rect.width;
            int sliderWidth = (int)slider.GetComponent<RectTransform>().rect.width;
            int spaceWidth = (sliderWidth - 5 * starWidth) / 5;
            int starsCount = iValue / 20;
            int starTotalWidth = sliderWidth - 5 * spaceWidth;
            res = ((starTotalWidth * value) + spaceWidth * starsCount) / sliderWidth;
            Debug.Log(string.Format("({0} * {1}) + {2} * {3} / {4}) = {5}", starTotalWidth, value, spaceWidth, starsCount, sliderWidth, res));
            return Mathf.Clamp(res, (starWidth / 2.0f) / sliderWidth, 1.0f);
        }
    }
}
