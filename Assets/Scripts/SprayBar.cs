using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SprayBar : MonoBehaviour
{

    public Slider spraySlider;
    public Gradient gradient;
    public Image fill;
    
    public void SetMaxSpray(float maxSpray)
    {
        spraySlider.maxValue = maxSpray;
        spraySlider.value = 0;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetSpray(float spray)
    {
        spraySlider.value = spray;
        fill.color = gradient.Evaluate(spraySlider.normalizedValue);
    }
}
