using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevelMusic", menuName = "LevelMusic")]
public class SOLevelMusic : ScriptableObject
{
    [SerializeField] public List<MusicTrackData> data;

    void OnValidate()
    {
        foreach(MusicTrackData track in data){
            track.CalculateTimings();
        }
    }
}
