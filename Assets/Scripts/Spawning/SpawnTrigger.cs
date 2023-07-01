using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField, HideInInspector] public string enounterLabel;

    SpawnManagerSingleton sms;

    void Awake()
    {
        sms = GameObject.Find("SpawnManagerSingleton").GetComponent<SpawnManagerSingleton>();
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            Debug.Log("Spawn " + enounterLabel + " triggered!");
            sms.SpawnTrigger(enounterLabel);
        }
    }


}

[CustomEditor(typeof(SpawnTrigger))]
public class SpawnTriggerEditor : Editor
{
    string[] encounterLabels;
    int selectedEncounterIndex;

    void OnEnable()
    {
        Encounter[] sceneEncounters = FindObjectsOfType<Encounter>();
        encounterLabels = new string[sceneEncounters.Length];
        for (int i = 0; i < sceneEncounters.Length; i++){
            encounterLabels[i] = sceneEncounters[i].label;
        }
    }

    public override void OnInspectorGUI()
    {
        SpawnTrigger trigger = (SpawnTrigger)target;

        base.OnInspectorGUI();

        ThisIsNotMadeByUnity.CustomInspector.Line(Color.grey);
        GUILayout.BeginHorizontal();
        if(encounterLabels.Length > 0){
            selectedEncounterIndex = System.Array.IndexOf(encounterLabels, trigger.enounterLabel);
            selectedEncounterIndex = EditorGUILayout.Popup(selectedEncounterIndex, encounterLabels);
            trigger.enounterLabel = encounterLabels[selectedEncounterIndex];
        }
        else{
            EditorGUILayout.Popup(0, new string[] {"No encounters found!"});
            Debug.LogWarning("No Enounters found in scene!");
        }
        EditorGUILayout.EndHorizontal();
    }
}
