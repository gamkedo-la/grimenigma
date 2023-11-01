using UnityEngine.UI;

public class PlayerPrefsSlider : PlayerPrefsUI
{
    void Start()
    {
        Slider slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener (delegate {
            OnValueChange(slider.value);
        });  
    }
}
