using UnityEngine;

public class GridItemData : MonoBehaviour
{
    public float paddingTop;
    public float paddingBottom;
    public float paddingLeft;
    public float paddingRight;

    public GridItemData(float paddingTop, float paddingBottom, float paddingLeft, float paddingRight)
    {
        this.paddingTop = paddingTop;
        this.paddingBottom = paddingBottom;
        this.paddingLeft = paddingLeft;
        this.paddingRight = paddingRight;
    }
}