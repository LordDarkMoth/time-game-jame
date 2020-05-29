using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class staminaUI : MonoBehaviour
{

    [SerializeField]
    Gradient gradient;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Image fill;


    public void SetUpBar(float maxStamina) {
        slider.maxValue = maxStamina;
        slider.value = maxStamina;
        fill.color = gradient.Evaluate(1f);
    }

    public void updateBar(float curStamina) {
        slider.value = curStamina;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
