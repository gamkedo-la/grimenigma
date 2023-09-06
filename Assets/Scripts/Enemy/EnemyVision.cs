using System.Collections;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    [Range(0f, 100f)][SerializeField] public float radius;
    [Range(0f, 360f)][SerializeField] public float angle;

    GameObject targetToFind;
    LayerMask targetMask;
    public LayerMask ObstructionMask;

    public bool canSeeTarget;

    // Start is called before the first frame update
    void Start()
    {
        targetToFind = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(RunSearchForTarget());
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
