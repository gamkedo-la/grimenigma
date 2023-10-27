using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Range(0f, 100f)][SerializeField] public float hardLandingThreshold;
    [SerializeField] VideoSetings videoSettings;
    [HideInInspector] public bool hasLandedThisCycle, isGrounded, isSliding;
    [HideInInspector] public float fov, mouseVerticleSensativity, gamepadVerticleSensativity, mouseHorizontalSensativity, gamepadHorizontalSensativity;
    [HideInInspector] public Vector3 landingVelocity;
    [HideInInspector] public GameObject leftItem, rightItem;

    void OnEnable()
    {
        videoSettings.onFovChange += UpdateFOV;
        videoSettings.onSensativityChange += UpdateSensativity;
    }

    void Start()
    {
        fov = PlayerPrefs.GetFloat("fov", PlayerPrefsDefault.defaultFov);
        gamepadVerticleSensativity = PlayerPrefs.GetFloat("gamepad_verticle_sensativity", PlayerPrefsDefault.defaultGamepadVerticleSensativity);
        gamepadHorizontalSensativity = PlayerPrefs.GetFloat("gamepad_horizontal_sensativity", PlayerPrefsDefault.defaultGamepadHorizontalSensativity);
        mouseVerticleSensativity = PlayerPrefs.GetFloat("mouse_verticle_sensativity", PlayerPrefsDefault.defaultMouseVerticleSensativity);
        mouseHorizontalSensativity = PlayerPrefs.GetFloat("mouse_horizontal_sensativity", PlayerPrefsDefault.defaultMouseHorizontalSensativity);
    }

    void OnDisable()
    {
        videoSettings.onFovChange -= UpdateFOV;
        videoSettings.onSensativityChange -= UpdateSensativity;
    }

    void UpdateFOV(float val)
    {
        fov = val;
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
