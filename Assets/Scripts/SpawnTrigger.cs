using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] EntitySpawner[] linkedSpawns;

    void OnTriggerEnter(Collider collidingObject)
    {
        if(collidingObject.gameObject.tag == "Player"){
            Debug.Log("Trigger spawn!");
            for(int i=0; i < linkedSpawns.Length; i++){
                linkedSpawns[i].TriggerSpawn();
            }
            this.gameObject.SetActive(false);
        }
    }
}
