using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownSelectable : MonoBehaviour, ISelectable
{
    public event Action<object> onChange;

    private TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void OnSelect()
    {
        // Add your logic here for when the dropdown is selected
        Debug.Log("Dropdown selected");
    }

    public void OnDeselect()
    {
        // Add your logic here for when the dropdown is deselected
        Debug.Log("Dropdown deselected");
    }

    public void Action(object data)
    {
        // Cast data to Vector2 and update the dropdown's value
        Vector2 vectorData = (Vector2)data;
        if (vectorData.y > 0)
        {
            // Navigate up
            dropdown.value = Mathf.Clamp(dropdown.value - 1, 0, dropdown.options.Count - 1);
        }
        else if (vectorData.y < 0)
        {
            // Navigate down
            dropdown.value = Mathf.Clamp(dropdown.value + 1, 0, dropdown.options.Count - 1);
        }
        Debug.Log("Dropdown action performed with data: " + data);
        InvokeOnChange(data);
    }

    public void InvokeOnChange(object data)
    {
        onChange?.Invoke(data);
    }
}