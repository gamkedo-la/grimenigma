using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BarrierEnemyAI : EnemyBaseAI
{
    [Header("BARRIER AI")]
    [SerializeField] float barrierCoolDown;
    [SerializeField] GameObject barrier;
    [SerializeField] private AudioClip attackBark;
    [SerializeField]
    [Range(0, 1)]
    private float attackBarkChance;
    private float attackBarkCooldown = 3f;
    
    bool barrierInUse = false;

    public override void HandleAlerted()
    {
        isPerformingAction = false;
        agentMove.ClearPath();
        HandleChase();
    }

    public override void HandleAttack()
    {
        Debug.Log("Attacking!");
        if (Random.Range(0f, 1f) < attackBarkChance) PlaySoundFX(attackBark);
        if(barrierInUse){ StartCoroutine(RunAttack(target.position, Random.Range(1, maxAttacks+1), weapon.cooldown)); }
        else{ StartCoroutine(RunSpawnBarrier(20)); }
    }

    public override void HandleChase()
    {
        Debug.Log("Chasing!");
        //CheckDistanceToTarget();
            agentMove.MaintainDistacne(target.position, maintainDistanceFromTarget);
            if(IsTargetWithinAttackRange()){ state = AIState.attack; }
            else{ state = AIState.chase; }
    }

    public override void OnBeginAttack()
    {
        PlaySoundFX(alertSound);
        StartCoroutine(BarkCooldown());
    }

    // Temporarily disables the attack bark by reducing its chance to zero.
    IEnumerator BarkCooldown()
    {
        var tmp = attackBarkChance;
        attackBarkChance = 0;
        yield return new WaitForSeconds(attackBarkCooldown);
        attackBarkChance = tmp;
    }

    IEnumerator RunSpawnBarrier(float distance)
    {
        barrierInUse = true;
        
        int tries = 0;
        Vector3 position;
        NavMeshHit hit;
        
        do{
            yield return new WaitForSeconds(0.001f);
            position = target.transform.position + Random.insideUnitSphere * distance;
            tries++;
            if(tries > 100){ position = transform.position; }
        }while(!NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas));

        //Debug.Log(hit.position);
        Instantiate(barrier, hit.position, transform.rotation);

        yield return new WaitForSeconds(barrierCoolDown);

        barrierInUse = false;
    }
}
