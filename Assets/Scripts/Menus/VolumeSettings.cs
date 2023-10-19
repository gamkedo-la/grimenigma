using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void SetMasterVolume(float val)
    {
        mixer.SetFloat("MasterVolume", val);
    }

    public void SetMusicVolume(float val)
    {
        mixer.SetFloat("MusicVolume", val);
    }

    public void SetFXVolume(float val)
    {
        mixer.SetFloat("FXVolume", val);
    }

    public void SetWeaponsVolume(float val)
    {
        mixer.SetFloat("WeaponsVolume", val);
    }

}
