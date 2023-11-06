#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridOrganizer))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridOrganizer gridOrganizer = (GridOrganizer)target;
        if (GUILayout.Button("Apply Grid Transformations"))
        {
            gridOrganizer.LayoutGrid();
        }
    }
}
#endif