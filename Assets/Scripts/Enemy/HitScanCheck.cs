using System.Collections;
using UnityEngine;

public class HitScanCheck : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] int damage;
    [SerializeField] float drawTime;
    [SerializeField] float attackCoolDown;

    Transform targetTransform;
    Vector3 positionOfCollision;
    bool shouldAttack = true;
    RaycastHit aimHit;
    RaycastHit attackHit;

    void Start(){
        targetTransform = GameObject.Find("Player/Body").transform;
    }

    // Update is called once per frame
    void Update(){
        if(shouldAttack){ AttackStart(); }
    }

    private void AttackStart(){
        //Debug.Log("Checking attack at" + targetTransform.position);
        StartCoroutine(ResetAttackCooldown());
        
        // DO SOMETHING TO CAUSE WAKE UP
        if(Physics.Raycast(transform.position, transform.forward, out aimHit, range)){
            //Debug.Log("Hit info:" + hit.collider.gameObject.name);
            StartCoroutine(HitCheck());
        }
    }

    IEnumerator HitCheck(){
        yield return new WaitForSeconds(drawTime);
        //Debug.Log("hit's transform:" +  hit.transform.position + " point of collision:" + positionOfCollision);
        //Debug.DrawRay(transform.position, transform.forward, Color.green, 2f);
        if (Physics.Raycast(transform.position, transform.forward, out attackHit, range)){
            //Debug.Log("Hit target:" + hit.transform.name);
            attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(damage, gameObject);
        }
    }

    IEnumerator ResetAttackCooldown(){
        shouldAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        shouldAttack = true;
    }
}
