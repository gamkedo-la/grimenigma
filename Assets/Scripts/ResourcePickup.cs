using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ResourcePickup : MonoBehaviour
{
    enum ResourceType{
        ammunition,
        armour,
        health,
        speed
    }

    [Header("Resource Settings")]
    [SerializeField] ResourceType pickupType = ResourceType.health;
    [SerializeField] int ammount = 1;
    [SerializeField] bool destroyOnPickup = true;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip fxSound;
    EquipmentHandler equipmentHandler;

    bool wasUsed;

    void Start()
    {
        equipmentHandler = GameObject.Find("Player/Camera/Weapons/ArmL").GetComponent<EquipmentHandler>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            switch (pickupType)
            {
                case ResourceType.ammunition:
                    HandleAmmunition(other);
                    break;
                case ResourceType.armour:
                    HandleArmour(other);
                    break;
                case ResourceType.health:
                    HandleHealth(other);
                    break;
                case ResourceType.speed:
                    HanndleSpeed(other);
                    break;
                default:
                    Debug.LogError("Invalid value for ResourceType enum!");
                    break;
            }

            if(wasUsed){
                PlaySoundFX();
                if(destroyOnPickup){ Destroy(this.gameObject, 0.1f); }
            }
        }
    }

    void HandleHealth(Collider other)
    {
        HealthController controller = other.transform.gameObject?.GetComponent<HealthController>();
        if(controller && controller.hp < controller.maxHP){
            controller.Heal(ammount);
            wasUsed = true;
        }
        //Debug.Log("Player HP:"  + other.transform.gameObject.GetComponent<HealthController>().hp);
    }

    void HandleAmmunition(Collider other)
    {
        //other.transform.gameObject.GetComponent<PlayerAttack>()?.CurrentWeapon.AddAmmo(ammount);
        AttackController weapon = equipmentHandler.currentEquipment?.GetComponent<AttackController>();
        if(weapon && weapon.ammo < weapon.maxAmmo && !weapon.infiniteAmmmo){
            weapon.AddAmmo(ammount);
            wasUsed = true;
        }

        //Debug.Log("Ammo item picked up");
    }

    void HandleArmour(Collider other)
    {
        HealthController controller = other.transform.gameObject?.GetComponent<HealthController>();
        if(controller && controller.armour < controller.maxArmour){
            controller.AddArmour(ammount);
            wasUsed = true;
        }
        //Debug.Log("Player Armour:"  + other.transform.gameObject.GetComponent<HealthController>().armour);
    }

    void HanndleSpeed(Collider other)
    {
        other.transform.gameObject.GetComponent<SpeedController>()?.ChangeSpeed((float) ammount);
        //Debug.Log("Player speed: " + other.gameObject.GetComponent<SpeedController>().speed);
        wasUsed = true;
    }

    void PlaySoundFX()
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
    }

}
