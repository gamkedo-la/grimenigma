using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuInput : MonoBehaviour
{
    PlayerInput input;
    Color deselectColor, selectedColor;
    List<Selectable> currentMenuOptions;
    int currentOptionIndex = 0;

    void Awake()
    {
        input = new PlayerInput();
        currentMenuOptions = new List<Selectable>(GetComponentsInChildren<Selectable>());
    }

    void OnEnable()
    {
        deselectColor = Color.white;//new Color(255, 104, 0, 255);
        selectedColor = new Color(255, 220, 0, 255);
        input.Enable();
        input.Menu.Confirm.performed += Confirm;
        input.Menu.Navigate.performed += Navigate;
        // Select the first option initially
        SelectOption(currentMenuOptions[currentOptionIndex]);
    }

    void OnDisable()
    {
        input.Menu.Confirm.performed -= Confirm;
        input.Menu.Navigate.performed -= Navigate;
        input.Disable();
    }

    void Update()
    {
        //if (Mouse.current.wasUpdatedThisFrame){ DeselectOption(currentMenuOptions[currentOptionIndex]); }
    }

    void Confirm(InputAction.CallbackContext obj)
    {
        // Check if the current option is a button
        if (currentMenuOptions[currentOptionIndex] is Button button){ button.onClick.Invoke(); }
    }

    void Navigate(InputAction.CallbackContext obj)
    {
        Vector2 navigateVector = obj.ReadValue<Vector2>();
        if (navigateVector.y > 0)
        {
            // Navigate up
            currentOptionIndex--;
            if (currentOptionIndex < 0){ currentOptionIndex = currentMenuOptions.Count - 1; }
        }
        else if (navigateVector.y < 0)
        {
            // Navigate down
            currentOptionIndex++;
            if (currentOptionIndex >= currentMenuOptions.Count){ currentOptionIndex = 0; }
        }
        else if (navigateVector.x != 0 && currentMenuOptions[currentOptionIndex] is Slider slider){
            slider.value += navigateVector.x;
        }

        // Select the current option
        SelectOption(currentMenuOptions[currentOptionIndex]);
    }

    void DeselectOption(Selectable option)
    {
        // Select the current option
        ColorBlock selectedColors = option.colors;
        selectedColors.normalColor = deselectColor;
    }

    void SelectOption(Selectable option)
    {
        // Deselect all options
        foreach (var opt in currentMenuOptions){
            ColorBlock colors = opt.colors;
            colors.normalColor = deselectColor;
            colors.highlightedColor = selectedColor;
            opt.colors = colors;
        }

        // Select the current option
        ColorBlock selectedColors = option.colors;
        selectedColors.normalColor = selectedColor;
        option.colors = selectedColors;
    }
}