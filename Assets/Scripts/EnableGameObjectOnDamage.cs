using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectOnDamage : MonoBehaviour
{
    [SerializeField] GameObject element;
    [SerializeField] HealthController pHeathController;
    [SerializeField] float flashTime;

    void OnEnable()
    {
        pHeathController.onDamage += Flash;
    }

    void OnDisable()
    {
        pHeathController.onDamage -= Flash;
    }

    void Flash(int damageAmmount, GameObject damageDealer)
    {
        StartCoroutine(OnFlashElement());
    }

    IEnumerator OnFlashElement()
    {
        element.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        element.SetActive(false);
    }
}
