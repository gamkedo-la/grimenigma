using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class EncounterListener : MonoBehaviour
{
    [HideInInspector] public string label;
    [HideInInspector] public bool wasTriggered;

    SpawnManagerSingleton sms;

    public event System.Action onEvent;
    
        void Awake()
    {
        sms = GameObject.Find("SpawnManagerSingleton").GetComponent<SpawnManagerSingleton>();
    }

    void Start()
    {
        sms.onSpawnTrigger += StartEncounter;
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
        if(triggeredLabel == label){ onEvent?.Invoke(); }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EncounterListener))]
public class EncounterListenerEditor : Editor
{
    string[] encounterLabels;
    int selectedEncounterIndex;

    void OnEnable()
    {
        EncounterTrigger[] sceneEncounters = FindObjectsOfType<EncounterTrigger>();
        encounterLabels = new string[sceneEncounters.Length];
        for (int i = 0; i < sceneEncounters.Length; i++){
            encounterLabels[i] = sceneEncounters[i].label;
        }
    }

    public override void OnInspectorGUI()
    {
        EncounterListener trigger = (EncounterListener)target;

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        if(encounterLabels.Length > 0){
            selectedEncounterIndex = System.Array.IndexOf(encounterLabels, trigger.label);
            if(selectedEncounterIndex < 0 || selectedEncounterIndex > encounterLabels.Length){
                Debug.LogError("Encounter index of " + selectedEncounterIndex + " out of bounds. Setting to 0!");
                selectedEncounterIndex = 0;
            }
            selectedEncounterIndex = EditorGUILayout.Popup(selectedEncounterIndex, encounterLabels);
            trigger.label = encounterLabels[selectedEncounterIndex];
        }
        else{
            EditorGUILayout.Popup(0, new string[] {"No encounters found!"});
            Debug.LogWarning("No Enounters found in scene!");
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif
