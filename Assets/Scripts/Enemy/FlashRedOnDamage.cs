using System.Collections;
using UnityEngine;

public class FlashRedOnDamage : MonoBehaviour
{
    private HealthController healthController;
    private Material material;
    private Color originalColor;

    void Start()
    {
        healthController = GetComponent<HealthController>();
        material = GetComponent<Renderer>().material;
        originalColor = material.color;

        healthController.onDamage += HandleOnDamage;
    }

    void HandleOnDamage(int damage, GameObject damageDealer)
    {
        StartCoroutine(FlashRed());
    }

    IEnumerator FlashRed()
    {
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = originalColor;
    }

    void OnDestroy()
    {
        healthController.onDamage -= HandleOnDamage;
    }
}