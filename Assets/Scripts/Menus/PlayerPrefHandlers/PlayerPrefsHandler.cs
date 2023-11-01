using UnityEngine;

public class PlayerPrefsHandler
{
    IPlayerPrefs playerPref;

    public PlayerPrefsHandler(PlayerPrefType dataType, string prefferenceName)
    {
        CreatePrefferenceFromType(dataType, prefferenceName);
    }

    public object GetValue()
    {
        return playerPref.PrefValue;
    }

    public object SetValue(object value)
    {
        playerPref.PrefValue = value;
        return playerPref.PrefValue;
    }

    void CreatePrefferenceFromType(PlayerPrefType type, string name)
    {
        switch (type)
        {
            case PlayerPrefType.Float:
                playerPref = new FloatPlayerPrefs(name);
                break;
            default:
                Debug.LogErrorFormat("{0} is invalid value for PlayerPrefType!", type);
                break;
        }
    }

}
