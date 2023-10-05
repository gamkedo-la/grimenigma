using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEditor;

public class NavMeshAgentMovement : MonoBehaviour
{
    [SerializeField] float patrolRange;
    [SerializeField] float maxDistanceFromCurrentPosition;
    [SerializeField] float minWaitTime, maxWaitTime;

    NavMeshAgent agent;
    Vector3 spawnPosition;
    Vector3 destination;

    bool isFindingPath = false;

    public void Patrol()
    {
        if(agent.remainingDistance <= agent.stoppingDistance && !isFindingPath){
            //Debug.Log("Finding new path!");
            StartCoroutine(RunSetNewValidPosition(transform.position, patrolRange, patrolRange, minWaitTime, maxWaitTime));
        }
    }

    public void MaintainDistacne(Vector3 targetPosition, float distance)
    {
        if(agent.remainingDistance <= agent.stoppingDistance && !isFindingPath){
            //Debug.Log("Moving away from player!");
            StartCoroutine(RunSetNewValidPosition(targetPosition, distance, distance, minWaitTime, maxWaitTime));
        }
    }

    public void ClearPath()
    {
        agent.ResetPath();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
    }

    IEnumerator RunSetNewValidPosition(Vector3 position, float searchRadius, float maxDistance, float minHoldTime, float maxHoldTime)
    {
        isFindingPath = true;

        NavMeshPath path = new NavMeshPath();
        Vector3 targetPosition;

        bool searchingForValidPosition = true;
        float yieldTime = 0.001f;
        float totalPathDistance = 0.0f;
        float holdTime = Random.Range(minHoldTime, maxHoldTime);

        do{ 
            targetPosition = transform.position + Random.insideUnitSphere * searchRadius;
            NavMesh.CalculatePath(position, targetPosition, NavMesh.AllAreas, path);
            if(path.status == NavMeshPathStatus.PathComplete){

                for(int i = 1; i < path.corners.Length; i++){
                    totalPathDistance += Vector3.Distance(path.corners[i-1], path.corners[i]);
                }
                
                if(totalPathDistance <= maxDistance){ searchingForValidPosition = false; }
            }
            else{
                searchRadius = searchRadius * 0.98f; 
                holdTime -= yieldTime;
                yield return new WaitForSeconds(yieldTime);
            }
            //Debug.Log(totalPathDistance);
        }while(searchingForValidPosition);

        yield return new WaitForSeconds(holdTime);

        agent.SetDestination(targetPosition);

        isFindingPath = false;
    }

    IEnumerator RunMaintainDisance(Vector3 position, float distance)
    {
        isFindingPath = true;
        Vector3 nextPosition = new Vector3();
        NavMeshHit hit;
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(waitTime);
        
        do{
            yield return new WaitForSeconds(0.001f);
            nextPosition = position + Random.insideUnitSphere * distance;
        }while(NavMesh.SamplePosition(nextPosition, out hit, 1f, NavMesh.AllAreas));

        agent.SetDestination(nextPosition);
        isFindingPath = false;
    }

}
