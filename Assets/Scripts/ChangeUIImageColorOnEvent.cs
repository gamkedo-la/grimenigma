using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class AbilityCooldown: UnityEvent<bool>{}

public class ChangeUIImageColorOnEvent : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public Color altColor;
    [HideInInspector] public Color baseColor;

    void Start()
    {
        baseColor = GetComponent<Image>().color;
    }

    public void UpdateColor(bool isAvailable)
    {
        switch (isAvailable)
        {
            case true:
                icon.color = baseColor;
                break;
            default:
                icon.color = altColor;
                break;
        }
    }
}
