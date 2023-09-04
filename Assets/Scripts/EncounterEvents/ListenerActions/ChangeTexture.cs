using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
    [SerializeField] Texture baseTexture;
    [SerializeField] Texture secondaryTexture;

    EncounterListener listener;
    Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        listener = GetComponent<EncounterListener>();
        listener.onEvent += Change;
    }

    void Change(string label)
    {
        if(label == listener.label){
            m_renderer.material.SetTexture("_MainTex", secondaryTexture);
        }
    }

}
