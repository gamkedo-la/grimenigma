using UnityEngine;

public class FloatPlayerPrefs : IPlayerPrefs
{
    public string PrefName { get; set; }

    public object PrefValue
    {
        get { return (float) PlayerPrefs.GetFloat(PrefName, PlayerPrefsDefault.Floats[PrefName]); }
        set { PlayerPrefs.SetFloat(PrefName, (float)value); }
    }

    public FloatPlayerPrefs(string name)
    {
        PrefName = name;
        PlayerPrefs.SetFloat(PrefName, PlayerPrefsDefault.Floats[PrefName]);
    }
}
