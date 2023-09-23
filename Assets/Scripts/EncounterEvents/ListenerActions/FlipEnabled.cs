using UnityEngine;

public class FlipEnabled : MonoBehaviour
{
    [SerializeField] bool onlyOnce;

    EncounterListener listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += FlipState;
    }

    void FlipState(string label)
    {
        if(listener.label == label){
            gameObject.SetActive(!gameObject.activeInHierarchy);
            if(onlyOnce){ this.enabled = !this.enabled; }
        }

    }

}
