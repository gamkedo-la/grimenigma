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
    }

    [SerializeField] ResourceType pickupType = ResourceType.health;
    [SerializeField] int ammount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
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
                default:
                    Debug.LogError("Invalid value for ResourceType enum!");
                    break;
            }
        }
    }


    void HandleHealth(Collider other){
        other.transform.gameObject.GetComponent<HealthController>()?.Heal(ammount);
        Debug.Log("Player HP:"  + other.transform.gameObject.GetComponent<HealthController>().hp);
    }

    
    void HandleAmmunition(Collider other){
        other.transform.gameObject.GetComponent<PlayerAttack>()?.CurrentWeapon.AddAmmo(ammount);
        Debug.Log("Ammo item picked up");
    }

    void HandleArmour(Collider other){
        // To Do: Implement Armour.
    }
}
