using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    EncounterTrigger myEvent;

    void Awake()
    {
        myEvent = GetComponent<EncounterTrigger>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Debug.Log("Spawn " + myEvent.label + " triggered!");
            myEvent.TriggerEvent();
            this.gameObject.SetActive(false);
        }
    }
}
