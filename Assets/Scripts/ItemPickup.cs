using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
public class ItemPickup : MonoBehaviour
{
    enum PickupAction{
        nothing,
        disable,
        destroy
    }

    [Header("Item Settings")]
    [SerializeField] ItemID item = ItemID.None;
    [SerializeField] int quantity = 1;
    [SerializeField] PickupAction onPickup = PickupAction.disable;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip fxSound;
    [SerializeField] GameObject spawnPrefabOnPickup;

    Collider myCollider;

    void Start()
    {
        myCollider = GetComponent<Collider>();
        myCollider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player"){ return; }
        
        if(other.gameObject.GetComponent<Inventory>()?.AddItem(item, quantity) == 1){
            //Debug.Log(this.name+ " Pickup Triggered!");
            PlaySoundFX();
            HandlePickup();
        } else {
            //Debug.Log(this.name+ " Duplicate Pickup Ignored!");
        }
    }

    void HandlePickup()
    {
        switch (onPickup)
        {
            case PickupAction.disable:
                gameObject.SetActive(false);
                break;
            case PickupAction.destroy:
                myCollider.enabled = false;
                if (spawnPrefabOnPickup) Instantiate(spawnPrefabOnPickup,transform.position,Quaternion.identity);
                Destroy(gameObject);
                break;
            case PickupAction.nothing:
                break;
            default:
                Debug.LogError("Invalid PickupAction value on " + gameObject + "!");
                break;
        }
    }

    void PlaySoundFX()
    {
        //Debug.Log("Playing sound " + fxSound.name);
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
    }
}
