using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    [Range(0f, 1000f)][SerializeField] public float radius;
    [Range(0f, 360f)][SerializeField] public float angle;
    [SerializeField] LayerMask ObstructionMask;

    [HideInInspector] public bool canSeeTarget;
    [HideInInspector] public float timeWithTarget;
    [HideInInspector] public GameObject targetToFind;

    LayerMask targetMask;

    // Start is called before the first frame update
    void Start()
    {
        targetMask = LayerMask.GetMask("Player");
        targetToFind = GameObject.FindGameObjectWithTag("Player");
        
        StartCoroutine(RunSearchForTarget());
    }

    void Update()
    {
        if(canSeeTarget){ timeWithTarget += Time.deltaTime; }
        else if(!canSeeTarget && timeWithTarget > 0.1f){ timeWithTarget = 0f; }
    }

    private void CheckForTarget()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, radius, targetMask);

        bool targetVisable = false;

        if(collisions.Length > 0){
            Transform currentTarget = collisions[0].transform;
            Vector3 targetDirection = (currentTarget.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, targetDirection) < angle/2){
                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);

                if(!Physics.Raycast(transform.position, targetDirection, distanceToTarget, ObstructionMask)){
                    targetVisable = true;
                }
            }
        }

        canSeeTarget = targetVisable;
    }

    private IEnumerator RunSearchForTarget()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while(true)
        {
            yield return wait;
            CheckForTarget();
        }
    }
}
