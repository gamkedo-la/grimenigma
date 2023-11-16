using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyVision : MonoBehaviour
{

    [Range(0f, 1000f)][SerializeField] public float radius;
    [Range(0f, 360f)][SerializeField] public float angle;
    [SerializeField] LayerMask ObstructionMask;

    [HideInInspector] public bool canSeeTarget = false;
    [HideInInspector] public float timeWithTarget;
    [HideInInspector] public GameObject targetToFind;

    LayerMask targetMask;

    // Start is called before the first frame update
    void Start()
    {
        targetMask = LayerMask.GetMask("Player");
        
        StartCoroutine(RunSearchForTarget());
    }

    void OnEnable()
    {
        canSeeTarget = false;
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

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyVision))]
public class EnemyVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyVision vision = (EnemyVision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(vision.transform.position, Vector3.up, Vector3.forward, 360, vision.radius);

        Vector3 viewAngle01 = DirectionFromAngle(vision.transform.eulerAngles.y, -vision.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(vision.transform.eulerAngles.y, vision.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle01 * vision.radius);
        Handles.DrawLine(vision.transform.position, vision.transform.position + viewAngle02 * vision.radius);

        if(vision.canSeeTarget){
            Handles.color = Color.green;
            Handles.DrawLine(vision.transform.position, vision.targetToFind.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
#endif