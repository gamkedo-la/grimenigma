using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField] SOLevelMusic music;

    [SerializeField] int intensity = 0;

    EncounterListener listener;
    MusicManager musicManager;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += RequestChangeMusic;

        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    void RequestChangeMusic(string label)
    {
        if(listener.label == label){ musicManager.ChangeSong(music, intensity); }
    }
}
