using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevelMusic", menuName = "LevelMusic")]
public class SOLevelMusic : ScriptableObject
{
    public List<MusicTrackData> data;

    void OnValidate()
    {
        foreach(MusicTrackData track in data){
            track.CalculateTimings();
        }
    }
}
