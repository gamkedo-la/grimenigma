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

    public event Action<float> onFovChange;
    public event Action<float, float, float, float> onSensativityChange;

    int currentResolutionIndex, currentFullScreenModeIndex;
    float fov, mouseVerticleSensativity, gamepadVerticleSensativity, mouseHorizontalSensativity, gamepadHorizontalSensativity;
    Resolution[] resolutions;
    List<string> fullScreenModeOptions = new List<string> {"Full Screen", "Full Screen Window", "Maximized Window", "Windowed"};

    public void AppyVideoSettings()
    {
        PlayerPrefs.SetFloat("fov", fov);
        onFovChange?.Invoke(fov);
        PlayerPrefs.SetFloat("mouse_verticle_sensativity", mouseVerticleSensativity);
        PlayerPrefs.SetFloat("mouse_horizontal_sensativity", mouseHorizontalSensativity);
        PlayerPrefs.SetFloat("gamepad_verticle_sensativity", gamepadVerticleSensativity);
        PlayerPrefs.SetFloat("gamepad_horizontal_sensativity", gamepadHorizontalSensativity);
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

        fov = PlayerPrefs.GetFloat("fov", PlayerPrefsDefault.defaultFov);
        gamepadVerticleSensativity = PlayerPrefs.GetFloat("gamepad_verticle_sensativity", PlayerPrefsDefault.defaultGamepadVerticleSensativity);
        gamepadHorizontalSensativity = PlayerPrefs.GetFloat("gamepad_horizontal_sensativity", PlayerPrefsDefault.defaultGamepadHorizontalSensativity);
        mouseVerticleSensativity = PlayerPrefs.GetFloat("mouse_verticle_sensativity", PlayerPrefsDefault.defaultMouseVerticleSensativity);
        mouseHorizontalSensativity = PlayerPrefs.GetFloat("mouse_horizonal_sensativity", PlayerPrefsDefault.defaultMouseHorizontalSensativity);
        onSensativityChange?.Invoke(mouseHorizontalSensativity, mouseVerticleSensativity, gamepadHorizontalSensativity, gamepadVerticleSensativity);

        RefreshMenuValues();
    }

    public void SetFOV(float val)
    {
        fov = val;
        onFovChange?.Invoke(fov);
    }

    public void SetMouseVerticleSensativity(float val)
    {
        mouseVerticleSensativity = val;
        onSensativityChange?.Invoke(mouseHorizontalSensativity, mouseVerticleSensativity, gamepadHorizontalSensativity, gamepadVerticleSensativity);
    }

    public void SetGamepadVerticleSensativity(float val)
    {
        gamepadVerticleSensativity = val;
        onSensativityChange?.Invoke(mouseHorizontalSensativity, mouseVerticleSensativity, gamepadHorizontalSensativity, gamepadVerticleSensativity);
    }
    public void SetMouseHorizontalSensativity(float val)
    {
        mouseHorizontalSensativity = val;
        onSensativityChange?.Invoke(mouseHorizontalSensativity, mouseVerticleSensativity, gamepadHorizontalSensativity, gamepadVerticleSensativity);
    }

    public void SetGamepadHorizontalSensativity(float val)
    {
        gamepadHorizontalSensativity = val;
        onSensativityChange?.Invoke(mouseHorizontalSensativity, mouseVerticleSensativity, gamepadHorizontalSensativity, gamepadVerticleSensativity);
    }

    public void SetResolution(int resolutionIndex)
    {
        Debug.Log("New resolution selected:" + resolutionIndex);
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
