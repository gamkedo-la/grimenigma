using UnityEngine;

public class PlayerPrefsUI : MonoBehaviour
{
    [SerializeField] PlayerPrefType dataType;
    [SerializeField] string preffenceName;

    PlayerPrefsHandler prefferenceHandler;

    System.Action<object> onPrefChange;

    public void OnValueChange(object value)
    {
        onPrefChange?.Invoke(value);
        prefferenceHandler.SetValue(value);
        Debug.Log(prefferenceHandler.GetValue());
    }

    void Awake()
    {
        prefferenceHandler = new PlayerPrefsHandler(dataType, preffenceName);
    }

}
