using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] public int intensity;
    [SerializeField] AudioSource[] sources = new AudioSource[2];

    bool changeMusic, isPaused, queueOneShot, playingOneShot;
    int lastIntensity, oneShotIntensity, sourceIndex, currentTrackFequency;
    double startTime, remainder, currentTrackLength, nextEndTime;
    double buffer = 0.5;
    
    MusicManagerState state, nextState, lastState;
    SOLevelMusic music, lastMusic, oneShot;

    enum MusicManagerState{
        ChangeTrack,
        Init,
        Playing,
        OneShot,
        Paused,
        Resume
    }
    
    public void ChangeSong(SOLevelMusic newMusic, int newIntensity)
    {
        Debug.LogFormat("Playing new music {0} at intensity {1}", newMusic.name, newIntensity);
        lastMusic = music;
        music = newMusic;
        SetIntensity(newIntensity);
    }

    public void PauseMusic()
    {
        nextState = MusicManagerState.Paused;
    }

    public void PlayOneShot(SOLevelMusic track, int newIntensity)
    {
        Debug.LogFormat("Playing one shot {0} at intensity {1}", track.data[newIntensity].Track.name, newIntensity);
        nextState = MusicManagerState.OneShot;
        oneShot = track;
        oneShotIntensity = newIntensity;
    }

    public void ResumeMusic()
    {
        nextState = MusicManagerState.Resume;
    }

    public void SetIntensity(int newIntensity)
    {
        nextState = MusicManagerState.ChangeTrack;
        intensity = newIntensity;
    }

    #region Unity Callback Funtions
    void Start()
    {
        state = MusicManagerState.Init;
    }

    void Update()
    {
        // HEY! This is not an ideal way to handle state managment. I'm crunching right now, but don't do this if it can be avoided.
        if(nextState != state){
            //Debug.LogFormat("state: {0}, nextState: {1}", state, nextState);
            TransitionState();
        }
        /*
        else if(state == MusicManagerState.Playing){
            QueueTrack(music.data[intensity]);
        }
        */

    }
    #endregion

    void TransitionState()
    {
        lastState = state;
        state = nextState;
        switch(state){
            case MusicManagerState.ChangeTrack:
                QueueTrack(music.data[intensity]);
                break;
            case MusicManagerState.OneShot:
                QueueTrack(oneShot.data[oneShotIntensity]);
                break;
            case MusicManagerState.Paused:
                Pause();
                break;
            case MusicManagerState.Resume:
                Resume();
                break;
            default:
                break;
        }
    }

    AudioSource GetNextAudioSource()
    {
        sourceIndex = 1 - sourceIndex;
        return sources[sourceIndex];
    }

    void Pause(){ 
        Debug.LogWarning("MusicManager Pause method was called, but is not implemented.");
    }

    void Resume()
    {
        Debug.LogWarning("MusicManager Resume method was called, but is not implemented.");
    }

    void QueueTrack(MusicTrackData nextTrack)
    {
        AudioSource currentSource = sources[sourceIndex];

        double currentTrackEndTime = 0;

        if(currentSource.isPlaying){
            Debug.LogFormat("{0},{1}", currentSource.time, currentSource.clip.frequency);
            remainder = currentSource.clip.samples / currentSource.clip.frequency;
            currentTrackEndTime = AudioSettings.dspTime + remainder;
            //Debug.LogFormat("Current track will end at: {0}. current time: {1}", currentTrackEndTime, AudioSettings.dspTime);
            currentSource.SetScheduledEndTime(currentTrackEndTime);
        }
        AudioSource nextSource = GetNextAudioSource();

        if(state == MusicManagerState.Paused || state == MusicManagerState.Init){
            currentTrackEndTime = AudioSettings.dspTime + buffer;
        }


        nextSource.clip = nextTrack.Track;
        nextSource.PlayScheduled(currentTrackEndTime);

        if(state != MusicManagerState.OneShot){ nextSource.loop = true; }
        else { nextSource.loop = false; }

        //Debug.LogFormat("Queued clip:{0}", nextSource.clip.name);

        currentTrackLength = nextTrack.Duration;
        currentTrackFequency = nextTrack.Track.frequency;

        QueueTrackNextState();
    }

    void QueueTrackNextState()
    {
        switch(state){
            case MusicManagerState.Paused:
                nextState = MusicManagerState.Paused;
                break;
            default:
                nextState = MusicManagerState.Playing;
                break;
        }

        //Debug.LogFormat("Next state {0}", nextState);
    }

}
