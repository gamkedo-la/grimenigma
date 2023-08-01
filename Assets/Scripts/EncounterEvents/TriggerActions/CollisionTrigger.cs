using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    [SerializeField] bool disableOnTrigger = true;
    
    [SerializeField] Collider myCollider;
    EncounterTrigger myEvent;

    void Awake()
    {
        myEvent = GetComponent<EncounterTrigger>();
        if(myCollider == null) { myCollider = GetComponent<Collider>(); }
    }

    void OnTriggerEnter(Collider other){
        //Debug.Log("Trigger collision with " + other.gameObject.name);
        if(other.tag == "Player"){
            Debug.Log("Event " + myEvent.label + " triggered!");
            myEvent.TriggerEvent();
            if(disableOnTrigger){ myCollider.enabled = false; }
            
        }
    }
}
