using UnityEngine;
using UnityEngine.UI;

public class SetRenderScale : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [Range(0.1f, 1)][SerializeField] float renderScale = 0.5f;

    Texture texture;

    #region Unity Callback Funtions
    void Awake()
    {
        texture = rawImage.texture;
    }
    void Start()
    {
        //Debug.LogFormat("renderScale:{0}", renderScale);

    }

    void Update()
    {
        texture.height = (int)(Screen.currentResolution.height * renderScale);

        texture.width = (int)(Screen.currentResolution.width * renderScale);
    }
    #endregion
}
