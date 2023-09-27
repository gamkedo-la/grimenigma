using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Unity.AI.Navigation;

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
            Debug.Log("Finding new path!");
            StartCoroutine(RunSetNewValidPosition());
        }
    }

    public void MaintainDistacne(Vector3 position, float distance)
    {
        if(agent.remainingDistance <= agent.stoppingDistance && !isFindingPath){
            Debug.Log("Moving away from player!");
            StartCoroutine(RunMaintainDisance(position, distance));
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

    IEnumerator RunSetNewValidPosition()
    {
        isFindingPath = true;
        Vector3 nextPosition = new Vector3();
        NavMeshHit hit;
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        yield return new WaitForSeconds(waitTime);
        
        do{
            nextPosition = transform.position + Random.insideUnitSphere * maxDistanceFromCurrentPosition;;
        }while(NavMesh.SamplePosition(destination, out hit, 1f, NavMesh.AllAreas));

        agent.SetDestination(nextPosition);
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
            nextPosition = position + Random.insideUnitSphere * distance;
        }while(NavMesh.SamplePosition(destination, out hit, 1f, NavMesh.AllAreas));

        agent.SetDestination(nextPosition);
        isFindingPath = false;
    }

}
