using UnityEngine;

public class FlipEnabled : MonoBehaviour
{
    [SerializeField] bool onlyOnce;
    [SerializeField] GameObject[] objectsToFlip;

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
            for(int i=0; i < objectsToFlip.Length; i++ ){
                objectsToFlip[i].SetActive(!objectsToFlip[i].activeInHierarchy);
                if(onlyOnce){ gameObject.SetActive(false); }
            }
        }

    }

}
