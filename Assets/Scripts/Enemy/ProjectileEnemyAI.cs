using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAI : EnemyBaseAI
{
    public override void HandleAttack()
    {
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


    private void Awake()
    {
        vision = GetComponent<EnemyVision>();
    }
}
