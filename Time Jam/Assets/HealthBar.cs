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
    Color32 redColor;
    [SerializeField]
    Color32 yellowColor;
    [SerializeField]
    Color32 greenColor;

    Slider slider;
    public void setHealth(int currentHealth) {
        slider.value = currentHealth;

        if (currentHealth < redBreakpoint) {
            barVisual.color = redColor;
        } else if (currentHealth < redBreakpoint) {
            barVisual.color = yellowColor;
        } else { 
        
        }

    }

    public void setUpBar(int maxHealth = 100) {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        yellowBreakpoint = (maxHealth / 10) * 6;
        redBreakpoint = maxHealth / 4;
    }
}
