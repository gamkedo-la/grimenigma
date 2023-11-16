using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] public int intensity;
    [SerializeField] AudioSource[] sources = new AudioSource[2];

    bool changeMusic, isPaused, queueOneShot, playingOneShot;
    int lastIntensity, oneShotIntensity, sourceIndex, currentTrackFequency;
    double startTime, remainder, currentTrackLength, nextEndTime;
    double currentDuration, currentBPM, currentBeatLength, currentSemiquaverLength, currentBarLength, currentBarDuration, currentMeasureDuration;
    bool currentIsInteruptable;
    double buffer = 0.2;
    double bodge_delayCompensation = .8d; // Prevents short delay in start time.
    
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
        //Debug.LogFormat("Playing new music {0} at intensity {1}", newMusic.name, newIntensity);
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
        //Debug.LogFormat("Playing one shot {0} at intensity {1}", track.data[newIntensity].Track.name, newIntensity);
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
        //Debug.LogFormat("newIntensity: {0}", newIntensity);
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
        if(sources[sourceIndex].timeSamples != 0){
            sourceIndex = 1 - sourceIndex;
        }

        return sources[sourceIndex];
    }

    void Pause(){ 
        Debug.LogWarning("MusicManager Pause method was called, but is not implemented.");
    }

    void Resume()
    {
        Debug.LogWarning("MusicManager Resume method was called, but is not implemented.");
    }

    void StoreTrackData(MusicTrackData track)
    {
        currentDuration = track.Duration;
        currentBPM = track.BPM;
        currentBeatLength = track.BeatLength;
        currentSemiquaverLength = track.SemiquaverLength;
        currentBarLength = track.BarLength;
        currentBarDuration = track.BarDuration;
        currentMeasureDuration = track.MeasureDuration;
        currentIsInteruptable = track.isInteruptable;

        //Debug.LogFormat("NewTimeData: Dur:{0}, BPM:{1}, BTL:{2}, SQL:{3}, BRL:{4}, BRD:{5}", currentDuration, currentBPM, currentBeatLength, currentSemiquaverLength, currentBarLength, currentBarDuration );
    }

    double CalculateScheduleTime()
    {
        AudioSource currentSource = sources[sourceIndex];

        //Debug.LogFormat("{0},{1}", currentSource.time, currentSource.clip.frequency);

        double timeElapsed;
        double timeToNext;
        double time;
        double interval;
        
        timeElapsed = currentSource.timeSamples / currentSource.clip.frequency; //pitch shifting will impact this!
        time = AudioSettings.dspTime;

        if(currentIsInteruptable){
            //Debug.LogFormat("{0}-{1}",currentBarDuration, currentMeasureDuration);
            interval = currentBeatLength;
            remainder = timeElapsed % interval;
            timeToNext = AudioSettings.dspTime + currentBarDuration - remainder;
        }
        else{
            interval = currentDuration;
            remainder = currentDuration - timeElapsed;
            timeToNext = AudioSettings.dspTime + remainder - bodge_delayCompensation;
        }
        
        //Debug.LogFormat("NextDelta:{0}, Time:{1}, Remainder:{2}", timeToNext, time, remainder);

        return timeToNext;
    }

    void QueueTrack(MusicTrackData nextTrack)
    {
        AudioSource currentSource = sources[sourceIndex];

        double nextTime = -1;
        if(lastState == MusicManagerState.Paused || lastState == MusicManagerState.Init){
            nextTime = AudioSettings.dspTime + buffer;
        }
        else if(currentSource.isPlaying){
            nextTime = CalculateScheduleTime();
            Debug.Log(nextTime);
            currentSource.SetScheduledEndTime(nextTime);
        }

        StoreTrackData(nextTrack);
        AudioSource nextSource = GetNextAudioSource();
        

        //Debug.LogFormat("nextTime: {0}", nextTime);
        nextSource.clip = nextTrack.Track;
        nextSource.PlayScheduled(nextTime);

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
