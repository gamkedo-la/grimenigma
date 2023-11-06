using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossairPlayerPrefsController : MonoBehaviour
{
    [SerializeField] PlayerPrefsDropdown type;
    [SerializeField] PlayerPrefsSlider scale;

    void OnEnable()
    {
        type.onChange += SetCrossairStyle;
        scale.onChange += SetCrossairScale;
    }

    void OnDisable()
    {
        type.onChange -= SetCrossairStyle;
        scale.onChange -= SetCrossairScale;
    }

    void SetCrossairStyle(object val)
    {
        
    }

    void SetCrossairScale(object val)
    {

    }
}
