using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(AttackController))]
public class MeleeEnemyAI : MonoBehaviour
{
    [Header("Attack")]
    [Range(0,5)][SerializeField] int aggressionLevel;
    [SerializeField] int maxAttacks;
    [SerializeField] float initialSpreadPenalty;
    [SerializeField] float imporoveSpreadIncrement;
    [SerializeField] AttackController weapon;
    [SerializeField] LayerMask whatIsTarget;
    [Header("Movement")]
    [SerializeField] float patrolRange;
    [SerializeField] float maintainDistanceFromTarget;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip alertSound;
    [Header("Bodge Settings")]
    [SerializeField] HealthController hpController;


    [SerializeField] LayerMask whatIsGround;


    EnemyVision vision;
    NavMeshAgentMovement agentMove;
    NavMeshAgent agent;
    AIState state;
    Transform target;
    Vector3 spawnPosition, walkPoint;
    
    bool hasWalkPoint, isPerformingAction, isAlerted;
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
                StartCoroutine(RunAttack(target.position, Random.Range(1, maxAttacks+1), weapon.cooldown));
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

    void CheckDistanceToTarget(){
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance - maintainDistanceFromTarget <= 0){
            // Might cause the enemy to attack when out of range.
            if(Random.Range(0, aggressionLevel) < aggressionLevel){ state = AIState.attack; }
            else { state = AIState.move; }
        }
    }

    void Patrol()
    {
        //Debug.Log("Patroling!");

        if(!hasWalkPoint){ GetNewPosition(); }
        if(hasWalkPoint){ agent.SetDestination(walkPoint); }
        if((transform.position - walkPoint).magnitude < 2f){ hasWalkPoint = false; }
        if(vision.canSeeTarget){ state = AIState.chase; }
    }

    void GetNewPosition()
    {
        //Debug.Log("Getting new position!");

        walkPoint  = new Vector3(
                                spawnPosition.x + Random.Range(-patrolRange, patrolRange),
                                spawnPosition.y + Random.Range(-patrolRange, patrolRange),
                                spawnPosition.z + Random.Range(-patrolRange, patrolRange)
                                );
        
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){ hasWalkPoint = true; }
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

    void AttackStart()
    {
        //Debug.Log("Attacking!");
        
        agent.SetDestination(transform.position);
        transform.LookAt(target);
        weapon.Attack();

        IsTargetWithinAttackRange();
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

    void RecievedDamage(int damage)
    {
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
}
