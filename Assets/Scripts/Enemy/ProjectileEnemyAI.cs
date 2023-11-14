using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAI : EnemyBaseAI
{
    public override void HandleAttack()
    {
        //Debug.DrawLine(transform.position, target.position, Color.red, 1);
        //Debug.LogFormat("Target transform:{0}", target.position);
        StartCoroutine(RunAttack(target.position, Random.Range(1, maxAttacks+1), weapon.cooldown));
    }
    public override void HandleAlerted()
    {
        HandleAttack();
    }
    public override void HandleChase()
    {
        HandleAttack();
    }

    public override void OnBeginAttack()
    {
        // do nothing
    }

    private void Awake()
    {
        vision = GetComponent<EnemyVision>();
    }
}
