using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey("master_volume");
        PlayerPrefs.DeleteKey("music_volume");
        PlayerPrefs.DeleteKey("fx_volume");
        PlayerPrefs.DeleteKey("weapon_volume");
        PlayerPrefs.Save();
    }


    public void AppyVolumeSettings()
    {
        float master, music, fx, weapons;
        mixer.GetFloat("MasterVolume", out master);
        mixer.GetFloat("MusicVolume", out music);
        mixer.GetFloat("FXVolume", out fx);
        mixer.GetFloat("WeaponsVolume", out weapons);

        PlayerPrefs.SetFloat("master_volume", master);
        PlayerPrefs.SetFloat("music_volume", music);
        PlayerPrefs.SetFloat("fx_volume", fx);
        PlayerPrefs.SetFloat("weapon_volume", weapons);

        PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetFloat("MasterVolume"));
    }

    public void SetVolumeSettings()
    {
       mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("master_volume",PlayerPrefsDefault.Floats["master_volume"]));
       mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("music_volume",PlayerPrefsDefault.Floats["music_volume"]));
       mixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("fx_volume",PlayerPrefsDefault.Floats["fx_volume"]));
       mixer.SetFloat("WeaponsVolume", PlayerPrefs.GetFloat("weapon_volume",PlayerPrefsDefault.Floats["weapon_volume"]));
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
