using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DeathController))]
[RequireComponent(typeof(AudioSource))]
public class BarrierEnemyAI : MonoBehaviour
{
    [Header("Attack")]
    [Range(0,5)][SerializeField] int aggressionLevel;
    [SerializeField] int maxAttacks;
    [SerializeField] float initialSpreadPenalty;
    [SerializeField] float imporoveSpreadIncrement;
    [SerializeField] AttackController weapon;
    [Header("Barrier")]
    [SerializeField] GameObject barrier;
    [SerializeField] float barrierCoolDown;
    [Header("Movement")]
    [SerializeField] float patrolRange;
    [SerializeField] float maintainDistanceFromTarget;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip alertSound;
    [Header("Bodge Settings")]
    [SerializeField] LayerMask whatIsTarget;
    [SerializeField] HealthController hpController;


    [SerializeField] LayerMask whatIsGround;


    EnemyVision vision;
    NavMeshAgentMovement agentMove;
    NavMeshAgent agent;
    AIState state;
    Transform target;
    Vector3 spawnPosition;
    
    bool isPerformingAction, isAlerted, barrierInUse;
    float currentSpread;

    void Start()
    {
        target = GameObject.Find("Player/Body").transform;
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
        agentMove = GetComponent<NavMeshAgentMovement>();

        spawnPosition = transform.position;
        state = AIState.idle;
        isAlerted = false;
    }

    void OnEnable()
    {
        hpController.onDamage += RecievedDamage;
    }

    void OnDisable()
    {
        hpController.onDamage -= RecievedDamage;
    }

    void Update()
    {
        if(target == null){target = gameObject.transform; }
        if(!isAlerted && vision.canSeeTarget){
            state = AIState.alerted;
            PlaySoundFX(alertSound);
            isAlerted = true;
            currentSpread = initialSpreadPenalty;
        }
        if(!isPerformingAction){
            CheckDistanceToTarget();
            CheckState();
        }

        if(vision.canSeeTarget && currentSpread > 0){
           currentSpread -= imporoveSpreadIncrement;
        }
        else if(!vision.canSeeTarget && currentSpread != initialSpreadPenalty){
            currentSpread = initialSpreadPenalty;
        }
    }

    void CheckState()
    {
        //https://www.youtube.com/watch?v=UjkSFoLxesw

        //Debug.Log(state);

        switch (state)
        {
            case AIState.alerted:
                HandleAlerted();
                break;
            case AIState.chase:
                ChaseTarget();
                break;
            case AIState.attack:
                HandleAttack();
                break;
            case AIState.move:
                // This does not really do what it says it does lol.
                state = AIState.chase;
                break;
            default:
                agentMove.Patrol();
                break;
        }
    }

    void HandleAlerted()
    {
        isPerformingAction = false;
        agentMove.ClearPath();
        ChaseTarget();
    }

    void HandleAttack()
    {
        if(barrierInUse){ StartCoroutine(RunAttack(target.position, Random.Range(1, maxAttacks+1), weapon.cooldown)); }
        else{ StartCoroutine(RunSpawnBarrier(20)); }
    }

    void CheckDistanceToTarget(){
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance - maintainDistanceFromTarget <= 0){
            // Might cause the enemy to attack when out of range.
            if(Random.Range(0, aggressionLevel) >= aggressionLevel){ state = AIState.attack; }
            else { state = AIState.move; }
        }
    }

    void ChaseTarget()
    {
        //Debug.Log("Chasing!");
        CheckDistanceToTarget();
        if(state != AIState.move){
            agent.SetDestination(target.position);
            IsTargetWithinAttackRange();
        }
    }

    void IsTargetWithinAttackRange()
    {
        if(Physics.CheckSphere(transform.position, weapon.range, whatIsTarget)){ state = AIState.attack; }
        else{ state = AIState.chase; }
    }

    void PlaySoundFX(AudioClip clip)
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(clip);
    }

    void RecievedDamage(int damage, GameObject damageSource)
    {
        // Will allow infighting, but EnemyVision needs to be updated beforehand.
        //if(damageSource.gameObject != null){ target = damageSource.gameObject.transform; }
        state = AIState.alerted;
    }

    IEnumerator RunAttack(Vector3 targetPosition, int repeat, float delayTime)
    {
        isPerformingAction = true;
        float baseWeaponSpread = weapon.spread;
        while(repeat > 0){
            transform.LookAt(targetPosition);
            weapon.spread = baseWeaponSpread + initialSpreadPenalty;
            weapon.Attack();
            repeat--;
            yield return new WaitForSeconds(delayTime);
        }
        state = AIState.move;
        weapon.spread = baseWeaponSpread;
        isPerformingAction = false;
    }

    IEnumerator RunSpawnBarrier(float distance)
    {
        barrierInUse = true;

        Vector3 position = new Vector3();
        NavMeshHit hit;
        
        do{
            yield return new WaitForSeconds(0.001f);
            position = target.transform.position + Random.insideUnitSphere * distance;
        }while(!NavMesh.SamplePosition(position, out hit, 1f, NavMesh.AllAreas));

        Debug.Log(hit.position);
        Instantiate(barrier, hit.position, transform.rotation);

        yield return new WaitForSeconds(barrierCoolDown);

        barrierInUse = false;
    }
}
