using UnityEngine;

public class ChainTrigger : MonoBehaviour
{
    [SerializeField] bool disableOnTrigger = true;

    EncounterListener listener;
    EncounterTrigger myEvent;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        myEvent = GetComponent<EncounterTrigger>();

        listener.onEvent += TriggerChainEvent;
    }

    void TriggerChainEvent(string label)
    {
        if(label == listener.label){
            myEvent.TriggerEvent();
            if(disableOnTrigger){ this.enabled = false; }
        }
    }
}
