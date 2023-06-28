using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField, HideInInspector] public string entityToSpawn;
    Object entityPrefab;

    void Start()
    {
        entityPrefab = Resources.Load("Prefabs/Enemies/" + entityToSpawn, typeof(GameObject));
        //TriggerSpawn();
    }

    public void TriggerSpawn()
    {
        Instantiate(entityPrefab, transform.position, transform.rotation);
    }
}

[CustomEditor(typeof(EntitySpawner))]
public class EntitySpawnEditor : Editor
{
    #region SerializedProperties
    //SerializedProperty entityToSpawn;
    #endregion

    [SerializeField, HideInInspector] int selectedEnemyIndex;
    List<string> enemyNameList = new List<string>();
    string[] enemyNames;

    void OnEnable()
    {
        //entityToSpawn = serializedObject.FindProperty("entityToSpawn");

        Object[] enemies = Resources.LoadAll("Prefabs/Enemies", typeof(GameObject));

        foreach(Object entity in enemies){
            enemyNameList.Add(entity.name);
        }

        enemyNames = enemyNameList.ToArray();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EntitySpawner spawnNode = (EntitySpawner)target;

        base.OnInspectorGUI();
        ThisIsNotMadeByUnity.CustomInspector.Line(Color.grey);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Spawner Custom Inspector");
        if (enemyNames.Length > 0){
            selectedEnemyIndex = enemyNameList.IndexOf(spawnNode.entityToSpawn);
            selectedEnemyIndex = EditorGUILayout.Popup(selectedEnemyIndex, enemyNames);
            spawnNode.entityToSpawn = enemyNames[selectedEnemyIndex];
        }
        else{
            EditorGUILayout.Popup(0, new string[] {"No Objects Found!"});
        }

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}
