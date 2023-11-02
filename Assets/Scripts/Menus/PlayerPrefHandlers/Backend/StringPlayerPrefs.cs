using UnityEngine;

public class StringPlayerPrefs : IPlayerPrefs
{
    public string PrefName { get; set; }

    public object PrefValue
    {
        get { return (string) PlayerPrefs.GetString(PrefName, PlayerPrefsDefault.Strings[PrefName]); }
        set { PlayerPrefs.SetString(PrefName, (string)value); }
    }

    public StringPlayerPrefs(string name)
    {
        PrefName = name;
        PlayerPrefs.SetString(PrefName, PlayerPrefsDefault.Strings[PrefName]);
        //Debug.Log(PlayerPrefs.String(PrefName, PlayerPrefsDefault.Strings[PrefName]));
    }
}
