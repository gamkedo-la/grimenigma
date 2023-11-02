using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject initialMenu;
    AudioSource sourceAudio;
    GameObject currentMenu;

    public void ApplyPlayerPrefs()
    {
        PlayerPrefs.Save();
    }

    public void Play()
    {
        // Uses scene order to load next scene after main menu.
        // Doc: https://docs.unity.cn/2019.1/Documentation/Manual/BuildSettings.html
        sourceAudio.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit button clicked.");
    }

    public void GoToMenu(GameObject menu)
    {
        currentMenu.SetActive(false);
        currentMenu = menu;
        currentMenu.SetActive(true);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void Start()
    {
        sourceAudio = GetComponent<AudioSource>();
        currentMenu = initialMenu;
        GoToMenu(currentMenu);
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    GameObject FindInChildren(string name)
    {
        foreach(Transform child in GetComponentsInChildren<Transform>(true)){
            if(child.gameObject.name == name){ return child.gameObject; }
        }

        Debug.LogError("Did not find child with name " + name + "!");
        // Probably a bad way to handle no result found, but it's whatever right now.
        return null;
    }
}
