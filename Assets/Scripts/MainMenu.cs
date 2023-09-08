using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioSource sourceAudio;

    void Start()
    {
        sourceAudio = GetComponent<AudioSource>();
    }
    public void Play()
    {
        // Uses scene order to load next scene after main menu.
        // Doc: https://docs.unity.cn/2019.1/Documentation/Manual/BuildSettings.html
        sourceAudio.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit button clicked.");
    }
}
