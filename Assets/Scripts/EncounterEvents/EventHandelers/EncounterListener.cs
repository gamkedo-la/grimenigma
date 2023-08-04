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

    public event System.Action<string> onEvent;
    
    void Awake()
    {
        sms = GameObject.Find("SpawnManagerSingleton").GetComponent<SpawnManagerSingleton>();
    }

    void OnEnable()
    {
        sms.onSpawnTrigger += StartEncounter;
    }

    void OnDisable()
    {
        sms.onSpawnTrigger -= StartEncounter;
    }

    void StartEncounter(string triggeredLabel)
    {
        if(triggeredLabel == label){ onEvent?.Invoke(triggeredLabel); }
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
        encounterLabels = new string[sceneEncounters.Length*2];
        EncounterListener listener = (EncounterListener)target;
        int j = 0;
        for (int i = 0; i < encounterLabels.Length; i+=2){
            encounterLabels[i] = sceneEncounters[j].label;
            encounterLabels[i+1] = sceneEncounters[j].endLabel;
            j++;
        }
    }

static EncounterListener lastUpdated = null;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EncounterListener listener = (EncounterListener)target;
        string lastLabel = listener.label;

        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if(encounterLabels.Length > 0){
            selectedEncounterIndex = System.Array.IndexOf(encounterLabels, listener.label);
            if(selectedEncounterIndex < 0 || selectedEncounterIndex > encounterLabels.Length){
                Debug.LogWarning("Encounter index of " + selectedEncounterIndex + " out of bounds. Setting to 0!");
                selectedEncounterIndex = 0;
            }
            selectedEncounterIndex = EditorGUILayout.Popup(selectedEncounterIndex, encounterLabels);
            listener.label = encounterLabels[selectedEncounterIndex];
            if(listener.label != lastLabel){
                EditorUtility.SetDirty(listener);
            }
        }
        else{
            EditorGUILayout.Popup(0, new string[] {"No encounters found!"});
            Debug.LogWarning("No Enounters found in scene!");
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif
