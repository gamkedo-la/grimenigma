using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManagerSingleton : MonoBehaviour
{
    EncounterTrigger[] events;
    //public delegate void AnnounceTrigger(string label);
    public static SpawnManagerSingleton sms;
    public event Action<string> onSpawnTrigger;

    void Awake()
    {
        events = FindObjectsOfType<EncounterTrigger>();
        foreach (EncounterTrigger trig in events)
        {
            trig.OnEventTrigger += SpawnTrigger;
        }
    }

    public void SpawnTrigger(string label)
    {
        onSpawnTrigger?.Invoke(label);
    }
}
