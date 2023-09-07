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


    float sightRange;
    Transform target;
    Transform targetTransform;
    Vector3 positionOfCollision;
    bool shouldAttack = true;
    RaycastHit aimHit;
    RaycastHit attackHit;

    bool targetInSightRange;
    bool isInCombat;

    Vector3 walkPoint;
    bool walkPointSet;
    float walkPointRange = 100;

    AIState state = AIState.idle;

    void Start()
    {
        target = GameObject.Find("Player/Body").transform;
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //https://www.youtube.com/watch?v=UjkSFoLxesw
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
        if(!walkPointSet){ GetNewPosition(); }
        if(walkPointSet){ agent.SetDestination(walkPoint); }
        if(vision.canSeeTarget){ state = AIState.chase; }
        if((transform.position - walkPoint).magnitude < 1){ walkPointSet = false; }
    }

    void GetNewPosition()
    {
        walkPoint  = new Vector3(
                                transform.position.x + Random.Range(-walkPointRange, walkPointRange),
                                transform.position.y + Random.Range(-walkPointRange, walkPointRange),
                                transform.position.z + Random.Range(-walkPointRange, walkPointRange)
                                );
        
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){ walkPointSet = true; }
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
