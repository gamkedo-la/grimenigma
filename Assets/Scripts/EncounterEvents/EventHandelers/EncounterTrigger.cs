using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection.Emit;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] public string label = "LABEL_ME!(>:|)=";
    [Header("!!!Make This <label>-end. I will automate this at some point.")]
    [SerializeField] public string endLabel;

    public event System.Action<string> OnEventTrigger;

    SpawnManagerSingleton sms;

    void Awake()
    {
        if(label == "LABEL_ME!(>:|)="){ Debug.LogError("TRIGGER MISSING LABEL!!"); }
        endLabel = label+"-end";
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
        EncounterListener[] Linkedlisteners = FindObjectsOfType<EncounterListener>();

        base.OnInspectorGUI();

        ThisIsNotMadeByUnity.CustomInspector.Line(Color.grey);
        GUILayout.BeginHorizontal();
        if(Linkedlisteners.Length > 0){
            foreach (EncounterListener listener in Linkedlisteners){
                if(listener.label == trigger.label){ EditorGUILayout.ObjectField(listener, typeof(EncounterListener), true); }
            }
        }

        EditorGUILayout.EndHorizontal();

        ThisIsNotMadeByUnity.CustomInspector.Line(Color.grey);
        GUILayout.BeginHorizontal();
        if(Linkedlisteners.Length > 0){
            foreach (EncounterListener listener in Linkedlisteners){
                if(listener.label == trigger.endLabel){ EditorGUILayout.ObjectField(listener, typeof(EncounterListener), true); }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
#endif