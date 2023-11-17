using UnityEngine;

public class ZombieEnemyAI : EnemyBaseAI
{
    public override void HandleAttack()
    {
        StartCoroutine(RunAttack(target.position, Random.Range(1, maxAttacks+1), weapon.cooldown));
    }
    public override void HandleAlerted()
    {
        isPerformingAction = false;
        agentMove.ClearPath();
        HandleChase();
    }
    public override void HandleChase()
    {
        agentMove.MaintainDistacne(target.position, maintainDistanceFromTarget);
        if(IsTargetWithinAttackRange()){
            if (animController) animController.SetBool("InRange", true);
            state = AIState.attack;
            }
        else{
            state = AIState.chase;
            if (animController) animController.SetBool("InRange", false);
        }
    }
    public override void OnBeginAttack()
    {
        PlaySoundFX(alertSound);
    }
}
