using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewLevelMusic", menuName = "LevelMusic")]
public class SOLevelMusic : ScriptableObject
{
    public List<MusicTrackData> data;
}
