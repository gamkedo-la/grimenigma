using UnityEngine;
using UnityEngine.UI;

public class ChangeUIImageColorOnEvent : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public Color altColor;

    [HideInInspector] public Color baseColor;

    void Awake()
    {
        baseColor = GetComponent<Image>().color;
    }

    public void SetColor(Color myColor)
    {
        icon.color = myColor;
    }
}
