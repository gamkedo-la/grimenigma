using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] bool requiresKey;
    [SerializeField] string keyName;

    bool isDoorClosed = true;
    EncounterListener listener;
    GameObject player;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += CheckDoorOpen;
        player = GameObject.Find("Player/Body");
    }

    void CheckDoorOpen(string label)
    {
        if(isDoorClosed && label == listener.label){
            if(!requiresKey || "Player has key check" != null){
                Debug.Log("We are opening the door " + this.gameObject.name + "!");
                transform.position = new Vector3(transform.position.x, transform.position.y+10, transform.position.z);
                isDoorClosed = false;
            }
        }
    }
}
