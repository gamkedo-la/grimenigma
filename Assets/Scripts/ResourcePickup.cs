using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.Collider))]
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

            PlaySoundFX();
            if(destroyOnPickup){ Destroy(this.gameObject, 0.1f); }
        }
    }

    void HandleHealth(Collider other)
    {
        other.transform.gameObject.GetComponent<HealthController>()?.Heal(ammount);
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
        //Debug.Log("Player HP:"  + other.transform.gameObject.GetComponent<HealthController>().hp);
    }

    void HandleAmmunition(Collider other)
    {
        other.transform.gameObject.GetComponent<PlayerAttack>()?.CurrentWeapon.AddAmmo(ammount);
        Debug.Log("Ammo item picked up");
    }

    void HandleArmour(Collider other)
    {
        other.transform.gameObject.GetComponent<HealthController>()?.AddArmour(ammount);
        Debug.Log("Player Armour:"  + other.transform.gameObject.GetComponent<HealthController>().armour);
    }

    void HanndleSpeed(Collider other)
    {
        other.transform.gameObject.GetComponent<SpeedController>()?.ChangeSpeed((float) ammount);
        Debug.Log("Player speed: " + other.gameObject.GetComponent<SpeedController>().speed);
    }

    void PlaySoundFX()
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
    }

}
