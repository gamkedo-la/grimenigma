using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] public Slider healthSlider, armourSlider, ammoSlider;
    [SerializeField] public Text healthText, armourText, ammoText;

    PlayerInputHanlder pInput;
    HealthController healthData;
    EquipmentHandler equipmentL, equipmentR;
    AttackController ammoDataL, ammoDataR;


    float currentHealth, currentArmour;
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
        SetMaxArmour(healthData.maxArmour);
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
        if(currentHealth != healthData.armour){ SetArmour(healthData.armour); }
        if(ammoDataL.infiniteAmmmo){
            ammoText.text = "Infin";
            ammoSlider.value = ammoDataL.maxAmmo;
        }
        else if(currentAmmo != ammoDataL.ammo){ SetAmmo(ammoDataL.ammo); }
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
        healthSlider.value = currentHealth;
    }

    void SetHealth(int health) {
        healthText.text = health.ToString();
        currentHealth = (float)health/100f;
        healthSlider.value = currentHealth;
    }

    void SetMaxArmour(int armour)
    {
        armourText.text = armour.ToString();
        currentArmour = (float)armour/100f;
        armourSlider.value = currentArmour;
    }

    void SetArmour(int armour) {
        armourText.text = armour.ToString();
        currentArmour = (float)armour/100f;
        armourSlider.value = currentArmour;
    }

    void SetMaxAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
        currentAmmo = (float)ammo;
        ammoSlider.value = currentAmmo;
    }

    void SetAmmo(int ammo) {
        ammoText.text = ammo.ToString();
        currentAmmo = (float)ammo;
        ammoSlider.value = currentAmmo;
    }
}
