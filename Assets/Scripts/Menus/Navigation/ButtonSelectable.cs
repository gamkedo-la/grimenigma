using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectable : MonoBehaviour, ISelectable
{
    public event Action<object> onChange;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnSelect()
    {
        // Add your logic here for when the button is selected
        Debug.Log("Button selected");
    }

    public void OnDeselect()
    {
        // Add your logic here for when the button is deselected
        Debug.Log("Button deselected");
    }

    public void Action(object data)
    {
        // Add your logic here for the action to be performed by the button
        Debug.Log("Button action performed with data: " + data);
        button.onClick.Invoke();
        InvokeOnChange(data);
    }

    public void InvokeOnChange(object data)
    {
        onChange?.Invoke(data);
    }
}
