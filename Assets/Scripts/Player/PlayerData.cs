using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Range(0f, 100f)][SerializeField] public float hardLandingThreshold;
    [SerializeField] PlayerPrefsUI fovPref;
    [SerializeField] VideoSetings videoSettings;
    [HideInInspector] public bool hasLandedThisCycle, isGrounded, isSliding;
    [HideInInspector] public float fov, mouseVerticleSensativity, gamepadVerticleSensativity, mouseHorizontalSensativity, gamepadHorizontalSensativity;
    [HideInInspector] public Vector3 landingVelocity;
    [HideInInspector] public GameObject leftItem, rightItem;

    public System.Action<float> onRefreshFOV;

    void OnEnable()
    {
        fovPref.onChange += RefreshFOV;
        videoSettings.onSensativityChange += UpdateSensativity;
    }

    void Start()
    {
        RefreshFOV();
        RefreshSensativity();
    }

    void OnDisable()
    {
        fovPref.onChange -= RefreshFOV;
        videoSettings.onSensativityChange -= UpdateSensativity;
    }

    void RefreshSensativity()
    {
        gamepadVerticleSensativity = PlayerPrefs.GetFloat("gamepad_verticle_sensativity", PlayerPrefsDefault.defaultGamepadVerticleSensativity);
        gamepadHorizontalSensativity = PlayerPrefs.GetFloat("gamepad_horizontal_sensativity", PlayerPrefsDefault.defaultGamepadHorizontalSensativity);
        mouseVerticleSensativity = PlayerPrefs.GetFloat("mouse_verticle_sensativity", PlayerPrefsDefault.defaultMouseVerticleSensativity);
        mouseHorizontalSensativity = PlayerPrefs.GetFloat("mouse_horizontal_sensativity", PlayerPrefsDefault.defaultMouseHorizontalSensativity);
    }

    void RefreshFOV()
    {
        fov = PlayerPrefs.GetFloat("fov", PlayerPrefsDefault.Floats["fov"]);
        onRefreshFOV?.Invoke(fov);
    }
    void RefreshFOV(object val)
    {
        // What a silly way to do this XD lol (comment written by the author of the code)
        RefreshFOV();
    }

    void UpdateSensativity(float mouseHori, float mouseVert, float gamepadHori, float gamepadVert)
    {
        //Debug.Log("Updating Sensativity.");
        mouseHorizontalSensativity = mouseHori;
        mouseVerticleSensativity = mouseVert;
        gamepadHorizontalSensativity = gamepadHori;
        gamepadVerticleSensativity = gamepadVert;
    }
}
