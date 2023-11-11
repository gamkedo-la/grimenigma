using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelMusic", menuName = "LevelMusic")]
[System.Serializable]
public class SOLevelMusic : ScriptableObject
{
    [SerializeField] public List<MusicTrackData> tracks;
    [SerializeField] public List<List<TrackMixerData>> mixerStates;
}
