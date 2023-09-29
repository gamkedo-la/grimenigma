using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider slider;
    public Text healthText, ammoText;

    HealthController healthData;
    PlayerAttack ammoData;

    float currentHealth;
    float currentAmmo;

    void OnEnable()
    {
        healthData = GameObject.Find("Player/Body").GetComponent<HealthController>();
        ammoData = GameObject.Find("Player/Body").GetComponent<PlayerAttack>();

        SetMaxHealth(healthData.baseHP);
        SetMaxAmmo(ammoData.CurrentWeapon.ammo);
    }

    void Update()
    {
        if(currentHealth != healthData.hp){ SetHealth(healthData.hp); }
        if(currentAmmo != ammoData.CurrentWeapon.ammo){ SetAmmo(ammoData.CurrentWeapon.ammo); }
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

    void SetMaxAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
        currentAmmo = (float)ammo;
        slider.value = currentAmmo;
    }

    void SetAmmo(int ammo) {
        ammoText.text = ammo.ToString();
        currentAmmo = (float)ammo;
        slider.value = currentAmmo;
    }
}
