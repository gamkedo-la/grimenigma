using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField] private int columnCount;
    [SerializeField] private float horizontalSpacing;
    [SerializeField] private float verticalSpacing;
    [SerializeField] private GameObject[] menuItems;

    public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
    public int ColumnCount { get { return columnCount; } }
    public float HorizontalSpacing { get { return horizontalSpacing; } }
    public float VerticalSpacing { get { return verticalSpacing; } }

    public GameObject[] GetSubMenuItems()
    {
        return menuItems;
    }
}
