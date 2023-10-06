using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DeathController))]
[RequireComponent(typeof(AudioSource))]
public class BarrierEnemyAI : EnemyBaseAI
{
    [Header("BARRIER AI")]
    [SerializeField] float barrierCoolDown;
    [SerializeField] GameObject barrier;
    
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

        Debug.Log(hit.position);
        Instantiate(barrier, hit.position, transform.rotation);

        yield return new WaitForSeconds(barrierCoolDown);

        barrierInUse = false;
    }
}
