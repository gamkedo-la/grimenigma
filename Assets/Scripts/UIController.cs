using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider slider;
    public Text healthText, ammoText;

    PlayerInputHanlder pInput;
    HealthController healthData;
    EquipmentHandler equipmentL, equipmentR;
    AttackController ammoDataL, ammoDataR;


    float currentHealth;
    float currentAmmo;

    void OnEnable()
    {
        GameObject pBody = GameObject.Find("Player/Body");
        pInput = pBody.GetComponent<PlayerInputHanlder>();
        healthData = pBody.GetComponent<HealthController>();
        equipmentL = GameObject.Find("Player/Camera/Weapons/ArmL").GetComponent<EquipmentHandler>();
        equipmentR = GameObject.Find("Player/Camera/Weapons/ArmR").GetComponent<EquipmentHandler>();
        GetCurrentWeapon(0);
        GetCurrentWeapon(1);

        SetMaxHealth(healthData.baseHP);
        SetMaxAmmo(ammoDataL.ammo);

        pInput.onWeaponSwap += GetCurrentWeapon;
    }

    void OnDisable()
    {
        pInput.onWeaponSwap -= GetCurrentWeapon;
    }

    void Update()
    {
        if(currentHealth != healthData.hp){ SetHealth(healthData.hp); }
        if(currentAmmo != ammoDataL.ammo){ SetAmmo(ammoDataL.ammo); }
    }

    void  GetCurrentWeapon(int armID)
    {
        switch (armID)
        {
            case 0:
                ammoDataL = equipmentL.currentEquipment.GetComponent<AttackController>();
                break;
            case 1:
                ammoDataR = equipmentR.currentEquipment.GetComponent<AttackController>();
                break;
            default:
                ammoDataL = equipmentL.currentEquipment.GetComponent<AttackController>();
                break;
        }
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
