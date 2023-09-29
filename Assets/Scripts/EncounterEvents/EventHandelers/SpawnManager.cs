using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManagerSingleton : MonoBehaviour
{
    EncounterTrigger[] events;
    EntitySpawner[] spawns;
    Dictionary<string, List<GameObject>> encounterTracker;
    int encounterEndCheckRate = 1;

    //public delegate void AnnounceTrigger(string label);
    public static SpawnManagerSingleton sms;
    public event Action<string> onSpawnTrigger;

    void Awake()
    {
        encounterTracker = new Dictionary<string, List<GameObject>>();

        events = FindObjectsOfType<EncounterTrigger>();
        foreach(EncounterTrigger trig in events){
            trig.OnEventTrigger += SpawnTrigger;
        }

        spawns = FindObjectsOfType<EntitySpawner>();
        foreach(EntitySpawner spawner in spawns){
            spawner.OnSpawn += AddEnemyToEncounter;
        }
    }

    public void SpawnTrigger(string label)
    {
        if(!encounterTracker.ContainsKey(label)){
            encounterTracker.Add(label, new List<GameObject>());
        }

        StartCoroutine(RunCheckForEncounterEnd(label));

        onSpawnTrigger?.Invoke(label);
    }

    public void AddEnemyToEncounter(string label, GameObject spawnedEnemy)
    {
        encounterTracker[label].Add(spawnedEnemy);
        //Debug.Log("Added enemy to encounter " + label + " Now " + encounterTracker[label].Count);
    }

    IEnumerator RunCheckForEncounterEnd(string label)
    {
        yield return new WaitForSeconds(encounterEndCheckRate);
        while(encounterTracker[label].Count > 0){
            for (int i = 0; i < encounterTracker[label].Count; i++) {
                if(encounterTracker[label][i] == null){
                    encounterTracker[label].RemoveAt(i);
                    i--;
                }
            }
            //Debug.Log("Remaining enemies in encounter " + label + " " + encounterTracker[label].Count);
            yield return new WaitForSeconds(encounterEndCheckRate);
        }
        onSpawnTrigger.Invoke(label+"-end");
    }
}
