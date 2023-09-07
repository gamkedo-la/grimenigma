using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using GrimEnigma.EnemyStates;

[RequireComponent(typeof(EnemyVision))]
public class HItScanEnemyAI : MonoBehaviour
{
    [SerializeField] LayerMask whatIsGround, whatIsTarget;
    [SerializeField] AttackController weapon;
    [SerializeField] float attackRange;

    EnemyVision vision;
    NavMeshAgent agent;

    AIState state;

    Transform target;
    Vector3 walkPoint;
    bool hasWalkPoint;
    float walkPointRange = 100;

    void Start()
    {
        target = GameObject.Find("Player/Body").transform;
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();

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
        if((transform.position - walkPoint).magnitude < 1){ hasWalkPoint = false; }
        if(vision.canSeeTarget){ state = AIState.chase; }
    }

    void GetNewPosition()
    {
        //Debug.Log("Getting new position!");

        walkPoint  = new Vector3(
                                transform.position.x + Random.Range(-walkPointRange, walkPointRange),
                                transform.position.y + Random.Range(-walkPointRange, walkPointRange),
                                transform.position.z + Random.Range(-walkPointRange, walkPointRange)
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
        if(Physics.CheckSphere(transform.position, attackRange, whatIsTarget)){ state = AIState.attack; }
        else{ state = AIState.chase; }
    }
}
