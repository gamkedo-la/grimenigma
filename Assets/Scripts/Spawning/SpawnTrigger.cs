using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] EntitySpawner[] linkedSpawns;

    SpawnManagerSingleton sms;

    void OnAwake()
    {
        sms = GameObject.Find("SpawnMangerSingleton").GetComponent<SpawnManagerSingleton>();
    }


}

[CustomEditor(typeof(SpawnTrigger))]
public class SpawnTriggerEditor : Editor
{
    string[] encounterLabels;

    void OnEnable()
    {
        Encounter[] encounters = FindObjectsOfType<Encounter>();
        encounterLabels = new string[encounters.Length];
        for (int i = 0; i < encounters.Length; i++)
        {
            encounterLabels[i] = encounters[i].label;
        }
    }

    void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        
    }
}
