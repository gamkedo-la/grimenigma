using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicManagerIntensity : MonoBehaviour
{
    [SerializeField] int value;
    [SerializeField] bool triggerOnce = true;

    EncounterListener listener;
    MusicManager musicManager;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += SetIntensity;

        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    void SetIntensity(string label)
    {
        if(listener.label == label){
            musicManager.SetIntensity(value);
            if(triggerOnce){ Destroy(gameObject); }
        }
    }
}
