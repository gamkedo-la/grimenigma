using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using GrimEnigma.EnemyStates;

[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(AttackController))]
public class HItScanEnemyAI : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGround, whatIsTarget;
    [SerializeField] AttackController weapon;
    [SerializeField] float patrolRange;
float attackRange;

    EnemyVision vision;
    NavMeshAgent agent;

    AIState state;

    Transform target;
    Vector3 spawnPosition, walkPoint;
    bool hasWalkPoint;

    void Start()
    {
        target = GameObject.Find("Player/Body").transform;
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();

        spawnPosition = transform.position;
        state = AIState.idle;
    }

    void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        //https://www.youtube.com/watch?v=UjkSFoLxesw

        //Debug.Log(state);

        switch (state)
        {
            case(AIState.chase):
                ChaseTarget();
                break;
            case(AIState.attack):
                AttackStart();
                break;
            default:
                Patrol();
                break;
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

        agent.SetDestination(target.position);
        IsTargetWithinAttackRange();
    }

    void AttackStart()
    {
        //Debug.Log("Attacking!");
        
        agent.SetDestination(transform.position);
        transform.LookAt(target);
        weapon.Attack();

        IsTargetWithinAttackRange();
    }

    void IsTargetWithinAttackRange(){
        if(Physics.CheckSphere(transform.position, weapon.range, whatIsTarget)){ state = AIState.attack; }
        else{ state = AIState.chase; }
    }
}
