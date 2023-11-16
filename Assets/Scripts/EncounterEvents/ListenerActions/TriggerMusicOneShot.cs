using UnityEngine;

public class TriggerMusicOneShot : MonoBehaviour
{
    [SerializeField] SOLevelMusic music;

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
        if(listener.label == label){ musicManager.PlayOneShot(music, intensity); }
    }
}