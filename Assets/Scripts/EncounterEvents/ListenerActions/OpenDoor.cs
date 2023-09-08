using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimations))]
public class OpenDoor : MonoBehaviour
{
    [Header("Your Door Is In Another Game Object")]
    [SerializeField] bool doorIsSeperateFromCollider;
    [SerializeField] GameObject doorGameObject;
    [Header("Key Logic")]
    [SerializeField] bool requiresItem = false;
    [SerializeField] ItemID item;
    [Header("Animation")]
    [Header("Transform")]
    [SerializeField] bool shouldTransform;
    [SerializeField] float distanceX;
    [SerializeField] float distanceY;
    [SerializeField] float distanceZ;
    [SerializeField] float speed;
    [Header("Rotatation")]
    [SerializeField] bool shouldRotate;
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;
    [SerializeField] float rotateSpeed;
    [Header("Closing")]
    [SerializeField] bool autoClose = true;
    [SerializeField] float secondsBeforeAutoClose;

    ScriptedAnimations sa;

    Vector3 closedPosition;
    Vector3 openPosition;
    bool isDoorClosed = true;
    EncounterListener listener;
    GameObject player, door;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        player = GameObject.Find("Player/Body");
        listener.onEvent += CheckDoorOpen;
    }

    void Start()
    {
        if(doorIsSeperateFromCollider){ door = doorGameObject; }
        else{ door = this.gameObject; }
        sa = door.GetComponent<ScriptedAnimations>();

        closedPosition = door.transform.position;
        openPosition = new Vector3(door.transform.position.x+distanceX, door.transform.position.y+distanceY, door.transform.position.z+distanceZ);
    }

    void CheckDoorOpen(string label)
    {
        if(isDoorClosed && label == listener.label){
            if(!requiresItem || player.GetComponent<Inventory>().HasItem(item)){
                //Debug.Log("We are opening the door " + this.gameObject.name + "!");
                if(shouldTransform){ sa.Twean(speed, openPosition); }
                if(shouldRotate) { sa.Rwean(rotateSpeed, rotateX, rotateY, rotateZ); }
                isDoorClosed = false;
                if(autoClose){ StartCoroutine(RunCloseDoor()); }
            }
        }
    }

    IEnumerator RunCloseDoor()
    {
        yield return new WaitForSeconds(secondsBeforeAutoClose);
        //Debug.Log("We are closing the door " + this.gameObject.name + "!");
        sa.Twean(speed, closedPosition);
        isDoorClosed = true;
    }
}
