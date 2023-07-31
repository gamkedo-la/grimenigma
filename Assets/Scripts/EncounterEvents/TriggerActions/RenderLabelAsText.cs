using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TMPro.TextMeshPro))]
public class RenderLabelAsText : MonoBehaviour
{

    EncounterTrigger trigger;
    TMPro.TextMeshPro renderedTextLabel;

    void Awake()
    {
        renderedTextLabel = GetComponent<TMPro.TextMeshPro>();
        RemoveText();

        #if UNITY_EDITOR
            if(!Application.isPlaying){
                trigger = this.gameObject.GetComponent<EncounterTrigger>();
                SetText();
            }
        #endif
    }

    void RemoveText()
    {
        renderedTextLabel.text = "";
    }

    void SetText()
    {
        renderedTextLabel.text = trigger.label;
    }
}
