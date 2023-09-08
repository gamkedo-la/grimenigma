using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    float currentHealth;
    public Slider slider;
    public Text healthText, ammoText;

    public void SetMaxHealth(int health)
    {
        healthText.text = health.ToString();
        currentHealth = (float)health/100f;
        slider.value = currentHealth;
    }

    public void SetHealth(int health) {
        healthText.text = health.ToString();
        currentHealth = (float)health/100f;
        slider.value = currentHealth;
    }
}
