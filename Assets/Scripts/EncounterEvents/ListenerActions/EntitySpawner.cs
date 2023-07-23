using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EntitySpawner : MonoBehaviour
{
    [SerializeField, HideInInspector] public string entityToSpawn;
    Object entityPrefab;
    EncounterListener listener;

    public event System.Action<string, GameObject> OnSpawn;

    void Awake()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += TriggerSpawn;
    }

    void Start()
    {
        entityPrefab = Resources.Load("Prefabs/Enemies/" + entityToSpawn, typeof(GameObject));
        //TriggerSpawn();
    }

    public void TriggerSpawn(string label)
    {
        GameObject spawnedEnemy = Instantiate(entityPrefab, transform.position, transform.rotation) as GameObject;
        OnSpawn.Invoke(label, spawnedEnemy);
    }
}

#if UNITY_EDITOR
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
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Spawner Custom Inspector");
        if (enemyNames.Length > 0){
            selectedEnemyIndex = enemyNameList.IndexOf(spawnNode.entityToSpawn);
            if(selectedEnemyIndex < 0 || selectedEnemyIndex > enemyNames.Length){
                Debug.LogError("Enemy Index of " + selectedEnemyIndex + " out of bounds. Resetting to 0!");
                selectedEnemyIndex = 0;
            }
            //Debug.Log(selectedEnemyIndex);
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
#endif