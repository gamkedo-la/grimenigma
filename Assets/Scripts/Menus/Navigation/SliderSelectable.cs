using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderSelectable : MonoBehaviour, ISelectable
{
    [SerializeField] float increment = 1f;

    public event Action<object> onChange;

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void OnSelect()
    {
        // Add your logic here for when the slider is selected
        Debug.Log("Slider selected");
    }

    public void OnDeselect()
    {
        // Add your logic here for when the slider is deselected
        Debug.Log("Slider deselected");
    }

    public void Action(object data)
    {
        // Cast data to Vector2 and update the slider's value
        Vector2 vectorData = (Vector2)data;
        if (vectorData.y > 0){ slider.value += increment; }
        else if (vectorData.y < 0){ slider.value -= increment; }
        Debug.Log("Slider action performed with data: " + data);
        InvokeOnChange(data);
    }

    public void InvokeOnChange(object data)
    {
        onChange?.Invoke(data);
    }
}
