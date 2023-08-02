using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimations))]
public class OpenDoor : MonoBehaviour
{
    [SerializeField] bool requiresKey = false;
    [SerializeField] string keyName;
    [SerializeField] float distance;
    [SerializeField] float speed;
    [SerializeField] bool autoClose = true;
    [SerializeField] float secondsBeforeAutoClose;
    

    Vector3 closedPosition;
    Vector3 openPosition;
    bool isDoorClosed = true;
    EncounterListener listener;
    GameObject player;
    ScriptedAnimations sa;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        sa = GetComponent<ScriptedAnimations>();
        player = GameObject.Find("Player/Body");

        closedPosition = transform.position;
        openPosition = new Vector3(transform.position.x, transform.position.y+distance, transform.position.z);

        listener.onEvent += CheckDoorOpen;
    }

    void CheckDoorOpen(string label)
    {
        if(isDoorClosed && label == listener.label){
            if(!requiresKey || "Player has key check" != null){
                Debug.Log("We are opening the door " + this.gameObject.name + "!");
                sa.Twean(speed, openPosition);
                isDoorClosed = false;
                StartCoroutine(RunCloseDoor());
            }
        }
    }

    IEnumerator RunCloseDoor()
    {
        yield return new WaitForSeconds(secondsBeforeAutoClose);
        Debug.Log("We are closing the door " + this.gameObject.name + "!");
        sa.Twean(speed, closedPosition);
        isDoorClosed = true;
    }
}
