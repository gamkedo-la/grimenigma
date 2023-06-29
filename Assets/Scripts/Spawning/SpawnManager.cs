using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnManagerSingleton : MonoBehaviour
{
    SpawnTrigger[] linkedSpawns;
    public delegate void AnnounceTrigger(string label);
    public static event AnnounceTrigger onAnnounceTrigger;

    void Awake()
    {
        linkedSpawns = FindObjectsOfType<SpawnTrigger>();
    }

    public void RecieveTrigger(string label)
    {
        onAnnounceTrigger?.Invoke(label);
    }
}
