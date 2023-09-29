using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicTrackData
{
    [Range(0, 10)]public int intensity;
    public AudioClip audio;
}

public class MusicManager : MonoBehaviour
{
    [SerializeField] List<MusicTrackData> music;
    [SerializeField] AudioSource[] sources;

    public int intensity;

    int currentSource, nextSource;

    float updateRate = 0.2f;
    bool lastCombatState;

    // Start is called before the first frame update
    void Start()
    {
        intensity = 1;
        currentSource = 1;
        nextSource = 0;

        TransitionTrack();
        StartCoroutine(RunCheckIntensity());
    }

    void TransitionTrack()
    {
        foreach(MusicTrackData track in music){
            if(track.intensity == intensity){
                //Debug.Log(track.audio.name);
                sources[currentSource].Stop();

                sources[nextSource].clip = track.audio;
                sources[nextSource].Play();
            }
        }

        switch (currentSource)
        {
            case 0:
                currentSource = 1;
                nextSource = 0;
                break;
            case 1:
                currentSource = 0;
                nextSource = 1;
                break;
            default:
                currentSource = 0;
                nextSource = 1;
                break;
        }
    }

    IEnumerator RunCheckIntensity()
    {
        int lastIntensity = intensity;

        while(true){
            if(lastIntensity != intensity){
                lastIntensity = intensity;
                TransitionTrack();
            }
            yield return new WaitForSeconds(updateRate);
        }

    }

    IEnumerator RunFadeAudioSources()
    {
        float updateRate = 0.1f;
        float step = 0.02f;
        int increments = 50;

        for(int i=0; i < increments; i++){
            sources[currentSource].volume -= step;
            sources[nextSource].volume += step;
            yield return new WaitForSeconds(updateRate);
        }
        sources[currentSource].Stop();
    }
}
