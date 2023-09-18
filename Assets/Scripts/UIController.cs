using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider slider;
    public Text healthText, ammoText;

    HealthController healthData;

    float currentHealth;

    void OnEnable()
    {
        healthData = GameObject.Find("Player/Body").GetComponent<HealthController>();

        SetMaxHealth(healthData.baseHP);
    }

    void Update()
    {
        if(currentHealth != healthData.hp){ SetHealth(healthData.hp); }
    }

    void SetMaxHealth(int health)
    {
        healthText.text = health.ToString();
        currentHealth = (float)health/100f;
        slider.value = currentHealth;
    }

    void SetHealth(int health) {
        healthText.text = health.ToString();
        currentHealth = (float)health/100f;
        slider.value = currentHealth;
    }
}
