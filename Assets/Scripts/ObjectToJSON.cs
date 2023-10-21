using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectToJSON : MonoBehaviour
{
    public void ConvertToJSON<T>(T obj, string name)
    {
        string jsonData = JsonUtility.ToJson(obj);
        string filepath = Application.persistentDataPath + '/' + name;
        //System.IO.File.WriteAllText(filepath, jsonData);
        PlayerPrefs.SetString("AllData", jsonData);
        Debug.Log(jsonData);
        Debug.Log(PlayerPrefs.GetString("AllData", "NOTHING"));
    }

    public void LoadFromJson<T>(ref T obj, string name)
    {
        string filepath = Application.persistentDataPath + '/' + name;
        //string jsonData = System.IO.File.ReadAllText(filepath);
        string jsonData = PlayerPrefs.GetString("AllData", "NOTHING");
        obj = JsonUtility.FromJson<T>(jsonData);
    }
}
