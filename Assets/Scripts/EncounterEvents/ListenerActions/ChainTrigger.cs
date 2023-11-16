using UnityEngine;
using System.Collections;

public class ChainTrigger : MonoBehaviour
{
    [SerializeField] bool disableOnTrigger = true;
    [SerializeField] float delay = 0;

    string myLabel;

    EncounterListener listener;
    EncounterTrigger myEvent;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        myEvent = GetComponent<EncounterTrigger>();

        myLabel = listener.label;

        listener.onEvent += TriggerChainEvent;
    }

    void TriggerChainEvent(string label)
    {
        if(label == myLabel){
            StartCoroutine(RunDelay());
        }
    }

    void TriggerChainEvent()
    {
        myEvent.TriggerEvent();
        if(disableOnTrigger){ this.enabled = false; }
    }

    IEnumerator RunDelay()
    {
        yield return new WaitForSeconds(delay);
        TriggerChainEvent();
    }
}
