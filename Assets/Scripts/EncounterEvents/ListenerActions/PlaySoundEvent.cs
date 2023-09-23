using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundEvent : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] bool randomPitch;
    [Range(0f, 0.3f)][SerializeField] float pitchRange = 0f;
    [SerializeField] AudioSource sourceAudio;
    [SerializeField] AudioClip soundClip;
    EncounterListener listener;

    // Start is called before the first frame update
    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += PlaySound;
    }

    void PlaySound(string label)
    {
        if(label == listener.label){
            //Debug.Log("Playing sound on " + gameObject.name);
            if(randomPitch){ sourceAudio.pitch = Random.Range(-pitchRange, pitchRange); }
            sourceAudio.PlayOneShot(soundClip);
        }
    }
}
