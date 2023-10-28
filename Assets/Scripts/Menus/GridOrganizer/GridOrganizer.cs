using UnityEngine;

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

    internal void LayoutGrid()
    {
        int rowCount = Mathf.CeilToInt((float)menuItems.Length / columnCount);
        float cellWidth = horizontalSpacing;
        float cellHeight = verticalSpacing;

        for (int i = 0; i < menuItems.Length; i++)
        {
            GameObject gridItem = menuItems[i];

            // Apply padding to the current grid item
            ApplyGridItemDataPadding(gridItem);

            // Check if the current grid item has a submenu
            if (gridItem.GetComponent<SubMenu>() != null)
            {
                // Get the submenu object
                SubMenu subMenu = gridItem.GetComponent<SubMenu>();

                // Get the submenu items
                GameObject[] subMenuItems = subMenu.GetSubMenuItems();

                // Calculate the number of rows required to fit all the submenu items in a grid
                int subMenuRowCount = Mathf.CeilToInt((float)subMenuItems.Length / subMenu.ColumnCount);

                // Calculate the width and height of each cell in the submenu grid
                float subMenuCellWidth = (subMenu.RectTransform.rect.width - (subMenu.ColumnCount - 1) * subMenu.HorizontalSpacing) / subMenu.ColumnCount;
                float subMenuCellHeight = (subMenu.RectTransform.rect.height - (subMenuRowCount - 1) * subMenu.VerticalSpacing) / subMenuRowCount;

                // Position each submenu item in its corresponding cell
                for (int j = 0; j < subMenuItems.Length; j++)
                {
                    GameObject subMenuItem = subMenuItems[j];

                    RectTransform subMenuItemRectTransform = subMenuItem.GetComponent<RectTransform>();
                    Vector2 offset = CalculateGridItemOffset(subMenuItem, subMenuCellWidth, subMenuCellHeight);
                    Vector2 position = gridItem.GetComponent<RectTransform>().anchoredPosition + offset + new Vector2(subMenuItemRectTransform.rect.width / 2, -subMenuItemRectTransform.rect.height / 2);
                    subMenuItemRectTransform.anchoredPosition = position;

                    // Apply padding to the current submenu item
                    ApplyGridItemDataPadding(subMenuItem);
                }

                // Update the number of rows required to fit all the items in the main grid
                rowCount += subMenuRowCount;
            }
            else
            {
                RectTransform gridItemRectTransform = gridItem.GetComponent<RectTransform>();
                Vector2 offset = CalculateGridItemOffset(gridItem, cellWidth, cellHeight);
                Vector2 position = new Vector2(originPosition.x, originPosition.y) + offset + new Vector2(gridItemRectTransform.rect.width / 2, -gridItemRectTransform.rect.height / 2);
                gridItemRectTransform.anchoredPosition = position;
            }
        }

        // Adjust the height of the main grid to accommodate all the items and their spacing
        float height = rowCount * cellHeight + (rowCount - 1) * verticalSpacing;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    internal void ApplyGridItemDataPadding(GameObject gridItem)
    {
        // Check if the current menu item has GridItem data and apply padding if it does exist
        GridItemData gridItemData = gridItem.GetComponent<GridItemData>();
        if (gridItemData != null)
        {
            RectTransform gridItemRectTransform = gridItem.GetComponent<RectTransform>();
            if (!gridItemRectTransform.offsetMin.Equals(new Vector2(gridItemData.paddingLeft, gridItemData.paddingBottom)) && !gridItemRectTransform.offsetMax.Equals(new Vector2(-gridItemData.paddingRight, -gridItemData.paddingTop)))
            {
                Vector2 offsetMin = new Vector2(gridItemData.paddingLeft, gridItemData.paddingBottom);
                Vector2 offsetMax = new Vector2(-gridItemData.paddingRight, -gridItemData.paddingTop);
                gridItemRectTransform.offsetMin = offsetMin;
                gridItemRectTransform.offsetMax = offsetMax;
                gridItemRectTransform.sizeDelta += offsetMin + offsetMax;
            }

            // Check if the current menu item has a submenu
            if (gridItem.GetComponent<SubMenu>() != null)
            {
                // Get the submenu object
                SubMenu subMenu = gridItem.GetComponent<SubMenu>();

                // Get the submenu items
                GameObject[] subMenuItems = subMenu.GetSubMenuItems();

                foreach (GameObject subMenuItem in subMenuItems)
                {
                    RectTransform subMenuItemRectTransform = subMenuItem.GetComponent<RectTransform>();
                    if (!subMenuItemRectTransform.offsetMin.Equals(new Vector2(gridItemData.paddingLeft, gridItemData.paddingBottom)) && !subMenuItemRectTransform.offsetMax.Equals(new Vector2(-gridItemData.paddingRight, -gridItemData.paddingTop)))
                    {
                        Vector2 subOffsetMin = new Vector2(gridItemData.paddingLeft, gridItemData.paddingBottom);
                        Vector2 subOffsetMax = new Vector2(-gridItemData.paddingRight, -gridItemData.paddingTop);
                        subMenuItemRectTransform.offsetMin = subOffsetMin;
                        subMenuItemRectTransform.offsetMax = subOffsetMax;
                        subMenuItemRectTransform.sizeDelta += subOffsetMin + subOffsetMax;
                    }
                }
            }
        }
    }

    internal Vector2 CalculateGridItemOffset(GameObject gridItem, float subMenuCellWidth, float subMenuCellHeight)
    {
        int column = gridItem.transform.GetSiblingIndex() % columnCount;
        int row = gridItem.transform.GetSiblingIndex() / columnCount;

        RectTransform gridItemRectTransform = gridItem.GetComponent<RectTransform>();
        Vector2 offset = new Vector2(column * (horizontalSpacing + subMenuCellWidth), -row * (verticalSpacing + subMenuCellHeight));
        return offset;
    }
}