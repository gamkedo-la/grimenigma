using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void AppyVolumeSettings()
    {
        float master, music, fx, weapons;
        mixer.GetFloat("MasterVolume", out master);
        mixer.GetFloat("MusicVolume", out music);
        mixer.GetFloat("FXVolume", out fx);
        mixer.GetFloat("WeaponsVolume", out weapons);

        PlayerPrefs.SetFloat("MasterVolume", master);
        PlayerPrefs.SetFloat("MusicVolume", music);
        PlayerPrefs.SetFloat("FXVolume", fx);
        PlayerPrefs.SetFloat("WeaponsVolume", weapons);

        //Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
    }

    public void SetVolumeSettings()
    {
       mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
       mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
       mixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("FXVolume"));
       mixer.SetFloat("WeaponsVolume", PlayerPrefs.GetFloat("WeaponsVolume"));
    }

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
