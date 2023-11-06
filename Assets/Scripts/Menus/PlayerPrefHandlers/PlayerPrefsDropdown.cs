using UnityEngine;
using TMPro;

public class PlayerPrefsDropdown : PlayerPrefsUI
{
    TMP_Dropdown dropdown;

    void Start()
    {
        dropdown = this.GetComponent<TMP_Dropdown>();
        // Add listener for when the value of the Dropdown changes
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
    }

    // This function is called when the value of the Dropdown changes
    void DropdownValueChanged(TMP_Dropdown change)
    {
        // Get the currently selected item's text
        string selectedItem = change.options[change.value].text;
        //Debug.Log("Selected Item: " + selectedItem);
        OnValueChange((object) selectedItem);
    }
}