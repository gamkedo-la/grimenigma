using UnityEngine;

public class PlayerDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
