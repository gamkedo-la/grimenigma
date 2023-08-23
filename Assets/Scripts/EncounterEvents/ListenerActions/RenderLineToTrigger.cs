using UnityEngine;

[ExecuteInEditMode]
public class RenderLineToTrigger : MonoBehaviour
{
    [SerializeField] public GameObject TriggerLine;
    [SerializeField, HideInInspector] GameObject line;
    LineRenderer[] existingLines;
    EncounterListener listener;

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
        //Debug.Log("Destroying line: " + line);
        foreach(LineRenderer currentLine in GetComponentsInChildren<LineRenderer>()){
            if(Application.isPlaying){ Destroy(currentLine.gameObject); }
            else{ DestroyImmediate(currentLine.gameObject); }
        }
    }

    void DrawLineToTrigger()
    {
        //Debug.Log("Drawing line from " + this.gameObject.name + " to trigger.");
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