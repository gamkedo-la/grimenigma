using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEditor;
using System.Transactions;
using Unity.VisualScripting;

public class NavMeshAgentMovement : MonoBehaviour
{
    [SerializeField] float patrolRange;
    [SerializeField] float maxDistanceFromCurrentPosition;
    [SerializeField] float minWaitTime, maxWaitTime;

    NavMeshAgent agent;
    Vector3 spawnPosition, destination, lastKnownValidPath;

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
        lastKnownValidPath = transform.position;
    }

    IEnumerator RunSetNewValidPosition(Vector3 position, float searchRadius, float maxDistance, float minHoldTime, float maxHoldTime)
    {
        //Debug.Log("-COROUTINE START-");
        
        isFindingPath = true;
        
        Vector3 targetPosition;
        NavMeshPath path = new NavMeshPath();
        NavMeshHit hit = new NavMeshHit();

        float totalPathDistance = 0.0f;
        float holdTime = Random.Range(minHoldTime, maxHoldTime);
        bool searchingForValidPosition = true;
        bool targetOnNavMesh = false;
        int allowedTries = 100;
        int attmeptCount = 0;

        do{
            //Debug.Log("Getting new position!");
            targetPosition = transform.position + Random.insideUnitSphere * searchRadius;
            targetOnNavMesh = NavMesh.SamplePosition(targetPosition, out hit, 1f, NavMesh.AllAreas);
            NavMesh.CalculatePath(position, targetPosition, NavMesh.AllAreas, path);
            if(targetOnNavMesh && path.status == NavMeshPathStatus.PathComplete){
                //Debug.Log("Got valid path!");
                for(int i = 1; i < path.corners.Length; i++){
                    totalPathDistance += Vector3.Distance(path.corners[i-1], path.corners[i]);
                }
                
                if(totalPathDistance <= maxDistance){
                    //Debug.Log("Path is within range!");
                    lastKnownValidPath = targetPosition;
                    searchingForValidPosition = false;
                    break;
                }
            }

            attmeptCount ++;
            if(attmeptCount > allowedTries){
                //Debug.Log("Too many attempts. Breaking!");
                targetPosition = lastKnownValidPath;
                break;
            }
            yield return new WaitForSeconds(0.0001f);
        }while(searchingForValidPosition || attmeptCount < allowedTries);

        yield return new WaitForSeconds(holdTime);

        agent.SetDestination(targetPosition);

        isFindingPath = false;
        //Debug.Log("-COROUTINE END-");
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
