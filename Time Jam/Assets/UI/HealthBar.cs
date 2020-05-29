using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private int yellowBreakpoint;
    private int redBreakpoint;

    [SerializeField]
    Image barVisual;
    [SerializeField]
    Gradient gradient;

    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }


    public void setHealth(int currentHealth) {
        slider.value = currentHealth;
        barVisual.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void setUpBar(int maxHealth = 100) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        barVisual.color = gradient.Evaluate(1f);
       
    }
    public void hideMe() {
        gameObject.SetActive(false);
    }
    public void showMe()
    {
        gameObject.SetActive(true);
    }
}
