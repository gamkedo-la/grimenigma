using System;
using System.Collections;
using UnityEngine;

public class BarrierLogic : MonoBehaviour
{
    [SerializeField] public float ttl = 7;

    void OnEnable()
    {
        StartCoroutine(DestroyGameObject(ttl));
    }

    IEnumerator DestroyGameObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
