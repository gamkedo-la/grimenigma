using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using GrimEnigma.EnemyStates;

public class HItScanEnemyAI : MonoBehaviour
{
    [SerializeField] float sightRange;
    Transform target;
    [SerializeField] LayerMask whatIsGround, whatIsTarget;
    [SerializeField] float attackRange;
    [SerializeField] int damage;
    [SerializeField] float drawTime;
    [SerializeField] float attackCoolDown;

    NavMeshAgent agent;

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

    void Awake()
    {
        target = GameObject.Find("Player/Body").transform;
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
        if(Physics.CheckSphere(transform.position, sightRange, whatIsTarget)){ state = AIState.chase; }
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
    
    private void AttackStart()
    {
        //Debug.Log("Attacking!");
        //Debug.Log("Checking attack at" + targetTransform.position);
        agent.SetDestination(transform.position);
        transform.LookAt(target);

        StartCoroutine(ResetAttackCooldown());
        
        // DO SOMETHING TO CAUSE WAKE UP
        if(Physics.Raycast(transform.position, transform.forward, out aimHit, attackRange)){
            //Debug.Log("Hit info:" + hit.collider.gameObject.name);
            StartCoroutine(HitCheck());
        }

        IsTargetWithinAttackRange();
    }

    void IsTargetWithinAttackRange(){
        if(Physics.CheckSphere(transform.position, attackRange, whatIsTarget)){ state = AIState.attack; }
        else{ state = AIState.chase; }
    }

    IEnumerator HitCheck()
    {
        yield return new WaitForSeconds(drawTime);
        //Debug.Log("hit's transform:" +  hit.transform.position + " point of collision:" + positionOfCollision);
        //Debug.DrawRay(transform.position, transform.forward, Color.green, 2f);
        if (Physics.Raycast(transform.position, transform.forward, out attackHit, attackRange)){
            //Debug.Log("Hit target:" + hit.transform.name);
            attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(damage);
        }
    }

    IEnumerator ResetAttackCooldown()
    {
        shouldAttack = false;
        yield return new WaitForSeconds(attackCoolDown);
        shouldAttack = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
