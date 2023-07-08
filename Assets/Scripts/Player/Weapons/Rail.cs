using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] float range, attackCoolDown;
    [SerializeField] int damage;
    [SerializeField] PlayerCameraControl pCamera;

    bool shouldAttack = true;
    RaycastHit attackHit;

    public void Attack()
    {
        if (shouldAttack && Physics.Raycast(pCamera.transform.position, pCamera.transform.forward, out attackHit, range)){
            StartCoroutine(ResetAttackCooldown());
            //Debug.Log("Attack!");
            //Debug.DrawRay(pCamera.transform.position, pCamera.transform.forward, Color.blue, 2f);
            attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(damage);
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        shouldAttack = true;
    }
}
