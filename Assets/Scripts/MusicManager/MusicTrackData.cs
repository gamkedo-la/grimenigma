using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicTrackData
{
    public string mixerChannel;
    public List<int> intensities;
    public List<float> volumes;
    public AudioClip audio;
}