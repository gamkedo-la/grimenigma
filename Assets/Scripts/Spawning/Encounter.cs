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

    void OnEnable()
    {
        SpawnManagerSingleton.onAnnounceTrigger += CheckEncounter;
    }

    void OnDisable()
    {
        SpawnManagerSingleton.onAnnounceTrigger -= CheckEncounter;
    }

    public void CheckEncounter(string triggeredLabel)
    {
        if(triggeredLabel == label){ StartEncounter(); }
    }

    void StartEncounter()
    {
        Debug.Log("Triggered ecounter " + label + "!");
        for(int i=0; i < linkedSpawns.Length; i++){
            linkedSpawns[i].TriggerSpawn();
        }
    }
}
