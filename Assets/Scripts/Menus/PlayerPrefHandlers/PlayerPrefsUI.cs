using UnityEngine;

public class PlayerPrefsUI : MonoBehaviour
{
    [SerializeField] PlayerPrefType dataType;
    [SerializeField] string preffenceName;

    public System.Action<object> onChange;

    PlayerPrefsHandler prefferenceHandler;


    public void OnValueChange(object value)
    {
        onChange?.Invoke(value);
        prefferenceHandler.SetValue(value);
        Debug.Log(prefferenceHandler.GetValue());
    }

    void Awake()
    {
        prefferenceHandler = new PlayerPrefsHandler(dataType, preffenceName);
    }

}
