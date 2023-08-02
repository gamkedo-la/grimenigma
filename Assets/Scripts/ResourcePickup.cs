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

    [SerializeField] ResourceType pickupType = ResourceType.health;
    [SerializeField] int ammount = 1;
    [SerializeField] bool destroyOnPickup = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
    }


    void HandleHealth(Collider other){
        other.transform.gameObject.GetComponent<HealthController>()?.Heal(ammount);
        Debug.Log("Player HP:"  + other.transform.gameObject.GetComponent<HealthController>().hp);
        if(destroyOnPickup){ Destroy(this.gameObject); }
    }

    
    void HandleAmmunition(Collider other){
        other.transform.gameObject.GetComponent<PlayerAttack>()?.CurrentWeapon.AddAmmo(ammount);
        Debug.Log("Ammo item picked up");
        if(destroyOnPickup){ Destroy(this.gameObject); }
    }

    void HandleArmour(Collider other){
        other.transform.gameObject.GetComponent<HealthController>()?.AddArmour(ammount);
        Debug.Log("Player Armour:"  + other.transform.gameObject.GetComponent<HealthController>().armour);
        if(destroyOnPickup){ Destroy(this.gameObject); }
    }

    void HanndleSpeed(Collider other)
    {
        other.transform.gameObject.GetComponent<SpeedController>()?.ChangeSpeed((float) ammount);
        Debug.Log("Player speed: " + other.gameObject.GetComponent<SpeedController>().speed);
        if(destroyOnPickup){ Destroy(this.gameObject); }
    }
}
