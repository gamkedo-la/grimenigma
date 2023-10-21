using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class VideoSetings : MonoBehaviour
{

    public TMP_Dropdown windowModeDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Slider fovSlider;

    int currentResolutionIndex, currentFullScreenModeIndex;
    float fov;
    Resolution[] resolutions;
    List<string> fullScreenModeOptions = new List<string> {"Full Screen", "Full Screen Window", "Maximized Window", "Windowed"};

    public void AppyVideoSettings()
    {
        PlayerPrefs.SetFloat("fov", fov);
        PlayerPrefs.SetInt("resolution_height", Screen.currentResolution.height);
        PlayerPrefs.SetInt("resolution_width", Screen.currentResolution.width);
        PlayerPrefs.SetInt("full_screen_mode", (int)Screen.fullScreenMode);

        SetVideoSettings();
    }

    public void SetVideoSettings()
    {
        Resolution resolution = new Resolution();
        resolution.height = PlayerPrefs.GetInt("resolution_height", Screen.currentResolution.height);
        resolution.width = PlayerPrefs.GetInt("resolution_width", Screen.currentResolution.width);
        resolution.refreshRateRatio = Screen.currentResolution.refreshRateRatio;
        FullScreenMode screenMode = (FullScreenMode) PlayerPrefs.GetInt("full_screen_mode", 1);
        SetScreen(resolution, screenMode);

        fov = PlayerPrefs.GetFloat("fov", 90);

        RefreshMenuValues();
    }

    public void SetFOV(float val)
    {
        fov = val;
    }

    public void SetResolution(int resolutionIndex)
    {
        Debug.Log("New resotion selected:" + resolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        SetScreen(resolution, Screen.fullScreenMode);
    }

    public void SetWindowState(int fullScreenModeIndex)
    {
        string selectedMode = fullScreenModeOptions[fullScreenModeIndex];
        FullScreenMode newMode;

        switch (selectedMode)
        {
            case "Full Screen":
                newMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case "Full Screen Window":
                newMode = FullScreenMode.FullScreenWindow;
                break;
            case "Maximized Window":
                newMode = FullScreenMode.MaximizedWindow;
                break;
            case "Windowed":
                newMode = FullScreenMode.Windowed;
                break;
            default:
                newMode = FullScreenMode.Windowed;
                break;
        }
        Screen.fullScreenMode = newMode;
        SetScreen(Screen.currentResolution, newMode);
    }

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new List<string>();
        StringifyResolutions(Screen.resolutions, ref resolutionOptions);
        resolutionDropdown.AddOptions(resolutionOptions);

        windowModeDropdown.ClearOptions();
        windowModeDropdown.AddOptions(fullScreenModeOptions);

        RefreshMenuValues();
    }

    int CompareResolutions(Resolution source, Resolution target)
    {
        int sourceValue = source.width * source.height;
        int targetValue = target.width * target.height;

        if(sourceValue == targetValue){ return 0; }
        else if(sourceValue > targetValue){ return 1; }
        else{ return -1; }
    }

    int GetCurrentResolutionIndex(Resolution[] resolutions, Resolution currentResolution)
    {
        for(int i = 0; i < resolutions.Length; i++){
            if(CompareResolutions(resolutions[i], currentResolution) == 0){
                return i;
            }
        }

        // Returns negative value of index 0;
        return -resolutions.Length;
    }

    int GetCurrentFullScreenModeIndex()
    {
        string currentFullScreenMode;

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                currentFullScreenMode = "Full Screen";
                break;
            case FullScreenMode.FullScreenWindow:
                currentFullScreenMode = "Full Screen Window";
                break;
            case FullScreenMode.MaximizedWindow:
                currentFullScreenMode = "Maximized Window";
                break;
            case FullScreenMode.Windowed:
                currentFullScreenMode = "Windowed";
                break;
            default:
                currentFullScreenMode = "Windowed";
                break;
        }

        int index = fullScreenModeOptions.FindIndex(a => a.Contains(currentFullScreenMode));
        return index;
    }

    void RefreshMenuValues()
    {
        currentResolutionIndex = GetCurrentResolutionIndex(Screen.resolutions, Screen.currentResolution);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        currentFullScreenModeIndex = GetCurrentFullScreenModeIndex();
        windowModeDropdown.value = currentFullScreenModeIndex;
        windowModeDropdown.RefreshShownValue();

        fovSlider.value = fov;
    }

    void SetScreen(Resolution resolution, FullScreenMode screenMode)
    {
        Debug.Log("Setting screen with values:" + "," + resolution.width + "," +resolution.height + "," +screenMode);
        Screen.SetResolution(resolution.width, resolution.height, screenMode);
    }

    void StringifyResolutions(Resolution[] resolutions, ref List<string> list)
    {
        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            list.Add(option);
        }
    }
}
