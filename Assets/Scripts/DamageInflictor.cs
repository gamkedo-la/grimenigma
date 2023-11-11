using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class DamageInflictor : MonoBehaviour
{
    public int damagePerSecond = 1;
    // not working: the movement controller overrides these changes
    // public Vector3 PushBackStrength = new Vector3(0,-2000,0); 
    public string tagToMatch = "Player";

    // runs every physics tick we are inside the lava!
    void OnTriggerStay(Collider other) {
        
        // Debug.Log(gameObject.name+ " was touched by " + other.gameObject.name);
        
        if (other.gameObject.CompareTag(tagToMatch)) {
            
            //Debug.Log(other.gameObject.name+ " is going to take "+damagePerSecond+" damage!");
            
            // we divide damage by 60 because this fires every fixedUpdate (60fps)
            // FIXME? debounce? could be called 2x some frames etc
            other.gameObject.GetComponent<HealthController>()?.Damage(damagePerSecond/60, gameObject, false);

            // this seems to have no effect?!
            // other.gameObject.GetComponent<Rigidbody>()?.AddForce(PushBackStrength,ForceMode.Impulse);
            // nor does this: must be grounded or canjump flags in movement interfering
            // other.gameObject.GetComponent<Rigidbody>()?.velocity = new Vector3(0, PushBackStrength.y, 0);

            other.gameObject.GetComponent<PlayerMovement>()?.Jump(); // good enough! =)
            
            // optional sound
            GetComponent<AudioSource>()?.Play();
        }
    }

}
