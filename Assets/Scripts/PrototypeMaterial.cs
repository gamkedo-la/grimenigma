using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PrototypeMaterial : MonoBehaviour
{
    Material mat;

    void OnValidate(){
        SetTileScaling();
    }

    void Start(){
        SetTileScaling();
    }

    private void SetTileScaling(){
        mat = GetComponent<Renderer>().sharedMaterial;
        mat.mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
    }
}
