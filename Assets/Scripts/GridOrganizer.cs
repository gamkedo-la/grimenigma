using UnityEngine;
using UnityEngine.UI;

public class GridOrganizer : MonoBehaviour
{
    [SerializeField] private Vector2 originPosition;
    [SerializeField] private float horizontalSpacing;
    [SerializeField] private float verticalSpacing;
    [SerializeField] private int columnCount;
    [SerializeField] private GameObject[] menuItems;

    private void Start()
    {
        LayoutGrid();
    }

    private void LayoutGrid()
    {
        int rowCount = Mathf.CeilToInt((float)menuItems.Length / columnCount);
        float cellWidth = (GetComponent<RectTransform>().rect.width - (columnCount - 1) * horizontalSpacing) / columnCount;
        float cellHeight = (GetComponent<RectTransform>().rect.height - (rowCount - 1) * verticalSpacing) / rowCount;

        for (int i = 0; i < menuItems.Length; i++)
        {
            GameObject menuItem = menuItems[i];
            int column = i % columnCount;
            int row = i / columnCount;

            RectTransform menuItemRectTransform = menuItem.GetComponent<RectTransform>();
            menuItemRectTransform.anchoredPosition = new Vector2(originPosition.x + column * (cellWidth + horizontalSpacing), originPosition.y - row * (cellHeight + verticalSpacing));
        }
    }
}
