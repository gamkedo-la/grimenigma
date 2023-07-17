using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] public string label = "LABEL_ME!(>:|)=";

    public event System.Action<string> OnEventTrigger;

    SpawnManagerSingleton sms;

    void Awake()
    {
        if(label == "LABEL_ME!(>:|)="){ Debug.LogError("TRIGGER MISSING LABEL!!"); }
        sms = GameObject.Find("SpawnManagerSingleton").GetComponent<SpawnManagerSingleton>();
    }

    public void TriggerEvent()
    {
        OnEventTrigger?.Invoke(label);
    }


}

#if UNITY_EDITOR
[CustomEditor(typeof(EncounterTrigger))]
public class SpawnTriggerEditor : Editor
{
    EncounterListener[] Linkedlisteners;
    int selectedEncounterIndex;

    void OnEnable()
    {
        //EncounterListener[] Linkedlisteners = FindObjectsOfType<EncounterListener>();
    }

    public override void OnInspectorGUI()
    {
        EncounterTrigger trigger = (EncounterTrigger)target;

        base.OnInspectorGUI();

        ThisIsNotMadeByUnity.CustomInspector.Line(Color.grey);
        GUILayout.BeginHorizontal();
        EncounterListener[] Linkedlisteners = FindObjectsOfType<EncounterListener>();
        if(Linkedlisteners.Length > 0){
            foreach (EncounterListener listener in Linkedlisteners)
            {
                EditorGUILayout.ObjectField(listener, typeof(EncounterListener), true);
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