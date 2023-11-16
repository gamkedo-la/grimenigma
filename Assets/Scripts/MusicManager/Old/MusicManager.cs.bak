using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;


public class MusicManager : MonoBehaviour
{
    [SerializeField] public int intensity = 0;
    [SerializeField] AudioMixer mixer;
    [SerializeField] SOLevelMusic music;

    float step = 10f;
    float stepDelay = 0.01f;
    int lastIntensity;

    List<string> channels = new List<string>{ "Music1", "Music2", "Music3", "Music4", "Music5" };
    List<AudioSource> sources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        SafetyChecks();
        PopulateMixer(music.data.Count);
        UpdateMixer(intensity, true);
    }

    void Update()
    {
        if(intensity != lastIntensity){ UpdateMixer(intensity, false); }
    }

    void PopulateMixer(int ammount)
    {
        for(int i = 0; i < ammount; i++){
            // Create Audio Source
            GameObject audioObject = new GameObject("AudioSource");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            sources.Add(audioSource);

            // Attach clip to source
            audioSource.clip = music.data[i].audio;
            audioSource.loop = true;
            audioSource.Play();

            // Assign to mixer track
            AudioMixerGroup[] mixerGroups = mixer.FindMatchingGroups(music.data[i].mixerChannel);
            audioSource.outputAudioMixerGroup = mixerGroups[0];
        }
    }

    void SafetyChecks()
    {
        if(channels.Count == 0){ Debug.LogError("Channels list is empty!"); }
        if(channels.Count < music.data.Count){ Debug.LogWarningFormat("channel count of {0} is less than to track count of {1}! This may cause strange behavor.", channels.Count, music.data.Count); }
    }
    
    void UpdateMixer(int intensityLevel, bool snap)
    {
        //Debug.LogFormat("music data count: {0}", music.data.Count);
        foreach(MusicTrackData track in music.data){
            //Debug.LogFormat("track: {0}", track);
            int index = track.intensities.IndexOf(intensityLevel); 
            float targetVolume = -80f;
            float currentVolume;
            float direction = 1;

            if(index != -1){
                //mixer.SetFloat(track.mixerChannel, -80f);
                //StartCoroutine(RunFadeMixerVolume(track.mixerChannel, -80f, step, stepDelay));
                targetVolume =  track.volumes[index];
                //continue;
            }

            mixer.GetFloat(track.mixerChannel, out currentVolume);
            if(targetVolume < currentVolume){ direction = -1; }
            //Debug.LogFormat("Fading track {0} to volume {1} with direction of {2}.", track.audio.name, targetVolume, direction);
            if(snap){ mixer.SetFloat(track.mixerChannel, targetVolume); }
            else{ StartCoroutine(RunFadeMixerVolume(track.mixerChannel, targetVolume, step*direction, stepDelay)); }
        }
    }

    IEnumerator RunFadeMixerVolume(string channel, float target, float step, float delay)
    {
        float currentValue;
        mixer.GetFloat(channel, out currentValue);

        int itterations = Mathf.Abs((int) MathF.Ceiling((target-currentValue)/step));

        while(itterations > 0){
            currentValue += step;
            mixer.SetFloat(channel, currentValue);
            yield return new WaitForSeconds(delay);
            itterations--;
        }
    }
}
