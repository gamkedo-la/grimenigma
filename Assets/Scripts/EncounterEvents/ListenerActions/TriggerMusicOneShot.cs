using UnityEngine;
using System.Collections;

public class TriggerMusicOneShot : MonoBehaviour
{
    [SerializeField] SOLevelMusic music;
    [SerializeField] bool hasDelay = false;
    [SerializeField] float delay = 2;

    int intensity = 0;

    EncounterListener listener;
    MusicManager musicManager;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += RequestOneShot;

        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    void RequestOneShot(string label)
    {
        if(listener.label == label){ StartCoroutine(RunDelay()); }
    }

    void TriggerRequestOneShot()
    {
        musicManager.PlayOneShot(music, intensity); 
    }

    IEnumerator RunDelay()
    {
        if(hasDelay){
            yield return new WaitForSeconds(delay);
        }
        TriggerRequestOneShot();
    }
}