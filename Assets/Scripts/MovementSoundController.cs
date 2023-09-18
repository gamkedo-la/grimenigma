using System.Collections.Generic;
using UnityEngine;

public enum MovementStyle { Landing, Running, Swimming, Walking }
public enum MaterialSurfaceType { Dirt, Hard, Metal, Wet }

[System.Serializable]
public class MovementSoundData
{
    public MovementStyle moveStyle;
    public MaterialSurfaceType materialType;
    public AudioClip sound;
}

public class MovementSoundController : MonoBehaviour
{
    [SerializeField] float actorHeight;
    [SerializeField] AudioSource sourceAudio;
    [SerializeField] List<MovementSoundData> soundData;

    RaycastHit hit;

    Dictionary<MovementStyle, Dictionary<MaterialSurfaceType,List<AudioClip>>> sounds;

    public void PlaySound(MovementStyle movement)
    {
        MaterialSurfaceType surfaceType = MaterialSurfaceType.Hard;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, actorHeight)){
            if(hit.transform.gameObject.TryGetComponent<MaterialSurfaceID>(out var component)){
                surfaceType = component.id;
            }
        }

        AudioClip randomSound = sounds[movement][surfaceType][Random.Range(0,sounds[movement][surfaceType].Count)];
        PlayAudioClip(randomSound);
    }

    public void PlaySound(MovementStyle movement, float pitch)
    {
        MaterialSurfaceType surfaceType = MaterialSurfaceType.Hard;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, actorHeight)){
            if(hit.transform.gameObject.TryGetComponent<MaterialSurfaceID>(out var component)){
                surfaceType = component.id;
            }
        }
        
        AudioClip randomSound = sounds[movement][surfaceType][Random.Range(0,sounds[movement][surfaceType].Count)];
        PlayAudioClip(randomSound, pitch);
    }

    void Start()
    {
        actorHeight = actorHeight/2;

        sounds = new Dictionary<MovementStyle, Dictionary<MaterialSurfaceType, List<AudioClip>>>();
        PopulateSoundLookupDictionary();
    }

    void PlayAudioClip(AudioClip sound)
    {
        sourceAudio.pitch = Random.Range(0.9f, 1.1f);
        sourceAudio.PlayOneShot(sound);
    }

    void PlayAudioClip(AudioClip sound, float pitch)
    {
        sourceAudio.pitch = pitch;
        sourceAudio.PlayOneShot(sound);
    }

    void PopulateSoundLookupDictionary()
    {
        for(int i = 0; i < soundData.Count; i++){
            MovementStyle currStyle = soundData[i].moveStyle;
            MaterialSurfaceType currMatType = soundData[i].materialType;

            //Debug.Log(!sounds.ContainsKey(currStyle) || !sounds[currStyle].ContainsKey(currMatType));

            if(!sounds.ContainsKey(currStyle)){
                sounds[currStyle] = new Dictionary<MaterialSurfaceType, List<AudioClip>>();
            }
            if(!sounds[currStyle].ContainsKey(currMatType)){
                sounds[currStyle][currMatType] = new List<AudioClip>();
            }

            sounds[currStyle][currMatType].Add(soundData[i].sound);
        }

        //Debug.Log(sounds.Count);
    }
}