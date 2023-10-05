using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    [SerializeField] float range, attackCoolDown, chargeTime, shakeIncrement;
    [SerializeField] int damage;
    [SerializeField] PlayerCameraControl pCamera;

    bool charging;
    bool shouldAttack = true;

    Vector3 shakePosition;
    RaycastHit attackHit;

    private void Update()
    {
        if(charging){
            AnimationCharge();
        }
    }

    public void Attack()
    {
        if (shouldAttack){
            StartCoroutine(RunResetAttackCooldown());
            StartCoroutine(RunChargedAttack());
            //Debug.Log("Attack!");
            //Debug.DrawRay(pCamera.transform.position, pCamera.transform.forward, Color.blue, 2f);
        }
    }

    void AnimationCharge()
    {
        shakeIncrement = shakeIncrement*-50;
        shakePosition = new Vector3(
            transform.localPosition.x + shakeIncrement,
            transform.localPosition.y + shakeIncrement,
            transform.localPosition.z + shakeIncrement
        );

        transform.localPosition = shakePosition; //Vector3.Lerp(transform.localPosition, shakePosition, Time.deltaTime);
    }

    IEnumerator RunChargedAttack()
    {
        Debug.Log("Charging!");
        charging = true;
        yield return new WaitForSeconds(chargeTime);
        charging = false;
        Physics.Raycast(pCamera.transform.position, pCamera.transform.forward, out attackHit, range);
        attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(damage, gameObject);
    }

    IEnumerator RunResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        shouldAttack = true;
    }
}
