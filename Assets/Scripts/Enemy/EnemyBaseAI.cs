using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(NavMeshAgentMovement))]
[RequireComponent(typeof(EnemyVision))]
[RequireComponent(typeof(AttackController))]
[RequireComponent(typeof(HealthController))]
[RequireComponent(typeof(DeathController))]
[RequireComponent(typeof(AudioSource))]
public abstract class EnemyBaseAI : MonoBehaviour
{
    [Header("BASE AI")]
    [Header("Attack")]
    [Range(0,5)][SerializeField] public int aggressionLevel;
    [SerializeField] public int maxAttacks;
    [SerializeField] public float initialSpreadPenalty;
    [SerializeField] public float imporoveSpreadIncrement;
    
    [Header("Movement")]
    [SerializeField] public float patrolRange;
    [SerializeField] public float maintainDistanceFromTarget;
    [SerializeField] protected Animator animController;
    
    [Header("Audio")]
    [SerializeField] public AudioClip alertSound;
    [SerializeField] public AudioSource soundSource;
    [Header("Bodge Settings")]
    [SerializeField] public LayerMask whatIsTarget;
    
    [SerializeField] public LayerMask whatIsGround;

    [HideInInspector] public HealthController hpController;
    [HideInInspector] public AttackController weapon;
    [HideInInspector] public EnemyVision vision;
    [HideInInspector] public NavMeshAgentMovement agentMove;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public AIState state;
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 spawnPosition;
    
    [HideInInspector] public bool isPerformingAction, isAlerted;
    [HideInInspector] public float currentSpread;

    [SerializeField] public bool debugMode = false;

    public abstract void HandleAttack();
    public abstract void HandleAlerted();
    public abstract void HandleChase();
    public abstract void OnBeginAttack();

    void OnEnable()
    {
        vision = GetComponent<EnemyVision>();
        agent = GetComponent<NavMeshAgent>();
        agentMove = GetComponent<NavMeshAgentMovement>();
        hpController = GetComponent<HealthController>();
        weapon = GetComponent<AttackController>();
        target = GameObject.Find("Player/Body").transform;
        if (animController) animController.SetBool("HasTarget", target);

        hpController.onDamage += RecievedDamage;
    }

    void Start()
    {
        spawnPosition = transform.position;
        state = AIState.idle;
        isAlerted = false;
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

    public void CheckState()
    {
        //https://www.youtube.com/watch?v=UjkSFoLxesw

        //Debug.Log(state);

        switch (state)
        {
            case AIState.alerted:
                HandleAlerted();
                break;
            case AIState.chase:
                HandleChase();
                break;
            case AIState.attack:
                HandleAttack();
                break;
            case AIState.move:
                // This does not really do what it says it does lol.
                state = AIState.chase;
                HandleChase();
                break;
            default:
                agentMove.Patrol();
                break;
        }
    }

    public void CheckDistanceToTarget()
    {
        var prevState = state;
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance - maintainDistanceFromTarget <= 0){
            // Might cause the enemy to attack when out of range.
            if (Random.Range(0, aggressionLevel) <= aggressionLevel)
            {
                state = AIState.attack;
                if (animController) animController.SetBool("InRange", true);
            }
            else
            {
                state = AIState.chase;
                if (animController) animController.SetBool("InRange", false);
            }

            // Sorry this is not very clean logic, but I couldn't find a better place for it.
            if (prevState == AIState.idle && state == AIState.attack)
            {
                OnBeginAttack();
            }
        }
    }

    public bool IsTargetWithinAttackRange()
    {
        return Physics.CheckSphere(transform.position, weapon.range, whatIsTarget);
    }

    public void PlaySoundFX(AudioClip clip)
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(clip);
    }

    public void RecievedDamage(int damage, GameObject damageSource)
    {
        // Will allow infighting, but EnemyVision needs to be updated beforehand.
        //if(damageSource.gameObject != null){ target = damageSource.gameObject.transform; }
        state = AIState.alerted;
    }

    public IEnumerator RunAttack(Vector3 targetPosition, int repeat, float delayTime)
    {
        isPerformingAction = true;
        float baseWeaponSpread = weapon.spread;
        while(repeat > 0){
            transform.LookAt(targetPosition);
            weapon.spread = baseWeaponSpread + initialSpreadPenalty;
            weapon.Attack();
            if (animController) animController.SetTrigger("Attack");
            repeat--;
            yield return new WaitForSeconds(delayTime);
        }
        state = AIState.move;
        weapon.spread = baseWeaponSpread;
        isPerformingAction = false;
    }
}