using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    AudioSource sourceAudio;
    GameObject currentMenu, mainMenu, levelSelectMenu, settingsMenu;

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

    public void EnableMainMenu()
    {
        currentMenu.SetActive(false);
        currentMenu = mainMenu;
        currentMenu.SetActive(true);
    }

    public void EnableSettings()
    {
        currentMenu.SetActive(false);
        currentMenu = settingsMenu;
        currentMenu.SetActive(true);
        
    }

    void Start()
    {
        sourceAudio = GetComponent<AudioSource>();
        mainMenu = FindInChildren("MainMenu").gameObject;
        settingsMenu = FindInChildren("SettingsMenu").gameObject;
        currentMenu = mainMenu;

        EnableMainMenu();
    }

    GameObject FindInChildren(string name)
    {
        foreach(Transform child in GetComponentsInChildren<Transform>(true)){
            if(child.gameObject.name == name){
                return child.gameObject;
            }
        }

        Debug.LogError("Did not find child with name " + name + "!");
        // Probably a bad way to handle no result found, but it's whatever right now.
        return null;
    }
}
