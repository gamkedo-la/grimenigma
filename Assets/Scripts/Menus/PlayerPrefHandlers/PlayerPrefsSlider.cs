using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsSlider : PlayerPrefsUI
{
    Slider slider;

    void Start()
    {
        slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener (delegate {
            OnValueChange(slider.value);
        });  
    }

    void Update()
    {
        if(slider.value != (float)prefferenceHandler.GetValue()){ slider.value = (float)prefferenceHandler.GetValue(); }
    }
}
