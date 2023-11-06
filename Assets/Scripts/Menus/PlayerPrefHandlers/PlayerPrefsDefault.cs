using System.Collections.Generic;

public static class PlayerPrefsDefault
{
    public static float defaultGamepadHorizontalSensativity = 500;
    public static float defaultGamepadVerticleSensativity = 500;
    public static float defaultMouseHorizontalSensativity = 10;
    public static float defaultMouseVerticleSensativity = 10;
    public static float defaultFov = 100;

    public static readonly Dictionary<string, float> Floats = new Dictionary<string, float>
    {
        { "fov", 90 },
        { "scale_weapon", 97},
        { "mouse_verticle_sensativity", 7},
        { "mouse_horizontal_sensativity", 7},
        { "gamepad_verticle_sensativity", 100},
        { "gamepad_horizontal_sensativity", 100},
        { "master_volume", 0 },
        { "music_volume", 0 },
        { "fx_volume", 0 },
        { "weapon_volume", 0 },
        { "crossair_scale", 0.5f },
    };

    public static readonly Dictionary<string, string> Strings = new Dictionary<string, string>
    {
        { "crossair_style", "Cross" }
    };
}
