using UnityEngine;

[ExecuteInEditMode]
public class RenderLineToTrigger : MonoBehaviour
{
    [SerializeField] public GameObject TriggerLine;
    EncounterListener listener;
    [SerializeField, HideInInspector] GameObject line;

    void Awake()
    {
        DestroyExistingLine();
        #if UNITY_EDITOR
            if(!Application.isPlaying){
            listener = this.gameObject.GetComponent<EncounterListener>();
            DrawLineToTrigger();
            }
        #endif
    }

    void DestroyExistingLine()
    {
        Debug.Log("Destroying line: " + line);
        if(line){
            if(Application.isPlaying){ Destroy(line); }
            else{ DestroyImmediate(line); }
        }
    }

    void DrawLineToTrigger()
    {
        Debug.Log("Running in edit mode.");
        foreach(EncounterTrigger trigger in FindObjectsOfType<EncounterTrigger>()){
            if(trigger.label == listener.label || trigger.endLabel == listener.label){
                line = Instantiate(TriggerLine, parent:this.gameObject.transform);
                LineRenderer renderer = line.GetComponent<LineRenderer>();
                renderer.SetPosition(0, transform.position);
                renderer.SetPosition(1, trigger.transform.position);    
                break;
            }
        }
    }
}