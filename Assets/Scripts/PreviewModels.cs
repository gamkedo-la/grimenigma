using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewModels : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    [SerializeField] float previewTime;

    float timer;
    int index;

    void OnEnable()
    {
        timer = previewTime;
        index = 0;
        objects[index].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0){
            timer = previewTime;
            objects[index].gameObject.SetActive(false);
            index += 1;
            if(index >= objects.Length){ index = 0; }
            objects[index].gameObject.SetActive(true);
        }

        timer -= Time.deltaTime;
        Debug.Log("Timer: " + timer);
    }
}
