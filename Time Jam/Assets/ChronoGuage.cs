using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronoGuage : MonoBehaviour
{
    [SerializeField]
    Slider GuageSlider;

    public void setUpGuage(int maxValue) {
        GuageSlider.maxValue = maxValue;
        GuageSlider.value = 0;
    }

    public void adjustGuage(int amount) {
        GuageSlider.value = Mathf.Clamp(GuageSlider.value + amount, 0, GuageSlider.maxValue);
    }

    public void hideMe()
    {
        gameObject.SetActive(false);
    }
    public void showMe()
    {
        gameObject.SetActive(true);
    }
}
