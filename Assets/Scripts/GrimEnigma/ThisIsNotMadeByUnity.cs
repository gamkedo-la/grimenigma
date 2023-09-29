using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ThisIsNotMadeByUnity{

#if UNITY_EDITOR
public class CustomInspector
{
    public static void Line(Color color, int thickness = 2, int padding = 15)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        EditorGUI.DrawRect(r, color);
    }
}
#endif
}