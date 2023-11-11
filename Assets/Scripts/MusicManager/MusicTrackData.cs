using UnityEngine;

[System.Serializable]
public class MusicTrackData
{
    public int id;
    [Range(0, 10)]public int intensity;
    public AudioClip audio;
}