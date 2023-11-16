using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicTrackData
{
    [SerializeField] AudioClip _track;
    [SerializeField] public bool isInteruptable = true;
    public AudioClip Track {
        get { return _track; }
        private set
        {
            _track = value;
            CalculateDuration(_track);
        }
    }
    [SerializeField] public string mixerChannel;
    [SerializeField] public int intensity;
    [SerializeField] public float volume;
    [SerializeField] private int _beats;
    public int Beats {
        get { return _beats; }
        private set
        {
            _beats = value;
        }
    }
    [SerializeField] private int _subdivision;
    public int Subdivision {
        get { return _subdivision; } 
        private set
        {
            _subdivision = value;
        }
    }
    [SerializeField] private int _bpm;
    public int BPM {
        get{ return _bpm; }
        private set
        {
            _bpm = value;
        }
    }

    public double Duration { get; private set;}
    public double BeatLength { get; private set; }
    public double SemiquaverLength { get; private set; }
    public double BarLength { get; private set; }
    public double BarDuration {get; private set;}

    public void CalculateTimings()
    {
        CalculateDuration(Track);
        CalculateTiming(BPM, Beats, Subdivision);
    }

    void CalculateDuration(AudioClip track)
    {
        Duration = (double)track.samples / track.frequency;
    }

    void CalculateTiming(double bpm, int beats, int subdivision)
    {
        if( bpm == 0 || beats == 0 || subdivision == 0){
            Debug.LogWarningFormat("0 value found in MusicTrackData. Skipping calculating timing. bpm:{0}, beats:{1}, subdivision{2}", bpm, beats, subdivision);
            return;
        }
        BeatLength = 60d/bpm;
        SemiquaverLength = BeatLength/beats;
        BarLength = BeatLength * beats * (beats/subdivision);
        BarDuration = 60d/bpm * beats;
    }
}