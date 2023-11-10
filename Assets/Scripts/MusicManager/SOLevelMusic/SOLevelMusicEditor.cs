#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomEditor(typeof(SOLevelMusic))]
public class SOLevelMusicEditor : Editor
{
    SOLevelMusic scriptableObject;
    private List<bool> foldouts = new List<bool>();
    private bool parentFoldout = false;

    private void OnEnable()
    {
        scriptableObject = (SOLevelMusic)target;
        if(scriptableObject.mixerStates == null){
            scriptableObject.mixerStates = new List<List<TrackMixerData>>();
        }
        for (int i = 0; i < scriptableObject.mixerStates.Count; i++){
            foldouts.Add(false);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck(); 
        DrawDefaultInspector();

        parentFoldout = EditorGUILayout.Foldout(parentFoldout, "Mixer States", true);
        if(parentFoldout){
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            scriptableObject.mixerStates = ResizeList(scriptableObject.mixerStates);
            for (int i = 0; i < scriptableObject.mixerStates.Count; i++)
            {
                DrawFoldoutMenu(scriptableObject.mixerStates[i], foldouts, i, "List " + i);
                if(foldouts[i])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    for (int j = 0; j < scriptableObject.mixerStates[i].Count; j++)
                    {
                         DrawFoldoutElement(scriptableObject.mixerStates[i][j], foldouts, j, "Element " + j);
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        if (true || EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }

    private void DrawFoldoutElement<T>(T element, List<bool> foldoutList, int index, string title) where T : new()
    {
        while(foldoutList.Count <= index)
        {
            foldoutList.Add(false);
        }
        foldoutList[index] = EditorGUILayout.Foldout(foldoutList[index], title, true);
        if(foldoutList[index])
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(GUI.skin.box); // Start the vertical group with a box
            DrawFields(element);
            EditorGUILayout.EndVertical(); // End the vertical group
            EditorGUI.indentLevel--;
        }
    }

    private void DrawFoldoutMenu<T>(List<T> list, List<bool> foldoutList, int index, string title) where T : new()
    {
        while(foldoutList.Count <= index)
        {
            foldoutList.Add(false);
        }
        foldoutList[index] = EditorGUILayout.Foldout(foldoutList[index], title, true);
        if(foldoutList[index])
        {
            EditorGUI.indentLevel++;
            list = ResizeList(list);
            EditorGUILayout.BeginVertical(GUI.skin.box); // Start the vertical group with a box
            EditorGUILayout.EndVertical(); // End the vertical group
            EditorGUI.indentLevel--;
        }
    }

    private List<T> ResizeList<T>(List<T> list) where T : new()
    {
        int newSize = EditorGUILayout.IntField("Size", list.Count);
        while(newSize < list.Count){
            list.RemoveAt(list.Count - 1);
        }
        while(newSize > list.Count){
            list.Add(new T());
        }
        return list;
    }

    private void DrawFields(object obj)
    {
        // Use reflection to get the fields of the object
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach(FieldInfo field in fields){
            if(field.FieldType == typeof(int)){
                int value = (int)field.GetValue(obj);
                value = EditorGUILayout.IntField(field.Name, value);
                field.SetValue(obj, value);
            }
            else if(field.FieldType == typeof(float)){
                float value = (float)field.GetValue(obj);
                value = EditorGUILayout.FloatField(field.Name, value);
                field.SetValue(obj, value);
            }
            else if(field.FieldType == typeof(bool)){
                bool value = (bool)field.GetValue(obj);
                value = EditorGUILayout.Toggle(field.Name, value);
                field.SetValue(obj, value);
            }
            else if(field.FieldType == typeof(string)){
                string value = (string)field.GetValue(obj);
                value = EditorGUILayout.TextField(field.Name, value);
                field.SetValue(obj, value);
            }
            else if(field.FieldType == typeof(Vector2)){
                Vector2 value = (Vector2)field.GetValue(obj);
                value = EditorGUILayout.Vector2Field(field.Name, value);
                field.SetValue(obj, value);
            }
            else if(field.FieldType == typeof(Vector3)){
                Vector3 value = (Vector3)field.GetValue(obj);
                value = EditorGUILayout.Vector3Field(field.Name, value);
                field.SetValue(obj, value);
            }
        }
    }
}
#endif