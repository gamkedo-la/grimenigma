using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCrossair : MonoBehaviour
{
    [SerializeField] Image crossair;
    [SerializeField] List<CrossairData> crossairs;
    [SerializeField] PlayerPrefsDropdown type;
    [SerializeField] PlayerPrefsSlider scale;

    void Start()
    {
        //Debug.LogFormat("crossairs count: {0}", crossairs.Count);
        SetCrossairStyle(PlayerPrefs.GetString("crossair_style", PlayerPrefsDefault.Strings["crossair_style"]));
        SetCrossairScale();
    }

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

    void SetCrossairStyle(string targetID)
    {
        foreach(CrossairData c in crossairs){
            if (c.id == targetID){ crossair.sprite = c.sprite; }
        }
    }

    void SetCrossairStyle(object id){ SetCrossairStyle((string) id); }

    void SetCrossairScale()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float scale = PlayerPrefs.GetFloat("crossair_scale", PlayerPrefsDefault.Floats["crossair_scale"]);
        rectTransform.localScale = new Vector2(scale, (float) scale);
        //Debug.LogFormat("{0},{1}", rectTransform, scale);
    }

    void SetCrossairScale(object scale){ SetCrossairScale(); }
}
