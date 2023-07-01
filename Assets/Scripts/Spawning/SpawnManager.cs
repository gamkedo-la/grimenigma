using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManagerSingleton : MonoBehaviour
{
    Encounter[] encounters;
    //public delegate void AnnounceTrigger(string label);
    public static SpawnManagerSingleton sms;
    public event Action<string> onSpawnTrigger;

    void Awake()
    {
        encounters = FindObjectsOfType<Encounter>();
        sms = this;
    }

    public void SpawnTrigger(string label)
    {
        onSpawnTrigger?.Invoke(label);
    }
}
