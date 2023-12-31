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
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip fxSound;
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
        
        // bugfix: we still use this sa - the other objexts are empty
        sa = door.GetComponent<ScriptedAnimations>();
        if (!sa) { // missing sa on it? try using the one built into this.
            sa = GetComponent<ScriptedAnimations>();
        }

        closedPosition = door.transform.position;
        openPosition = new Vector3(door.transform.position.x+distanceX, door.transform.position.y+distanceY, door.transform.position.z+distanceZ);
    }

    void CheckDoorOpen(string label)
    {
        if(isDoorClosed && label == listener.label){
            if(!requiresItem || player.GetComponent<Inventory>().HasItem(item)){
                //Debug.Log("We are opening the door " + this.gameObject.name + "!");
                if(shouldTransform && sa!=null){ sa.Twean(speed, openPosition); }
                if(shouldRotate && sa!=null) { sa.Rwean(rotateSpeed, rotateX, rotateY, rotateZ); }
                PlaySoundFX();
                isDoorClosed = false;
                if(autoClose){ StartCoroutine(RunCloseDoor()); }
            }
            //else { Debug.Log("Door: we don't have required item! Staying closed."); }
        }
        //else { Debug.Log("Door is already open or label: "+label+" isn't the same as the listener's: "+listener.label); }
    }

    void PlaySoundFX()
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
    }

    IEnumerator RunCloseDoor()
    {
        yield return new WaitForSeconds(secondsBeforeAutoClose);
        //Debug.Log("We are closing the door " + this.gameObject.name + "!");
        if (sa!=null) sa.Twean(speed, closedPosition);
        isDoorClosed = true;
    }
}
