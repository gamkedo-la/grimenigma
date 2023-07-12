using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    [SerializeField] public string label = "LABEL_ME!(>:|)=";
    [SerializeField] EntitySpawner[] linkedSpawns;

    SpawnManagerSingleton sms;

    void Awake()
    {
        if(label == "LABEL_ME!(>:|)="){ Debug.LogError("ENCOUNTER MISSING LABEL!!"); }
    }

    void Start()
    {
        SpawnManagerSingleton.sms.onSpawnTrigger += StartEncounter;
    }

    void OnEnable()
    {
        //SpawnManagerSingleton.onAnnounceTrigger += CheckEncounter;
    }

    void OnDisable()
    {
        //SpawnManagerSingleton.onAnnounceTrigger -= CheckEncounter;
    }

    void StartEncounter(string triggeredLabel)
    {
        if(triggeredLabel == label){
            for(int j=0; j < 5; j++){
            //Debug.Log("Triggered ecounter " + label + "!");
                for(int i=0; i < linkedSpawns.Length; i++){
                    linkedSpawns[i].TriggerSpawn();
                }
            }
        }
    }
}
