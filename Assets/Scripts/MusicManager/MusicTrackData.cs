using UnityEngine;

[System.Serializable]
public class MusicTrackData
{
    [Range(0, 10)]public int intensity;
    public AudioClip audio;
}