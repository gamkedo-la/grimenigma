using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfHTML5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_WEBGL
        Destroy(gameObject);
        #endif        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
