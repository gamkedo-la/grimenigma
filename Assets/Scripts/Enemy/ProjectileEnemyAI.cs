using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ProjectileEnemyAI : MonoBehaviour
{

    enum ProjectileEnemyState { Searching, Tracking }

    [SerializeField] Transform eyeball;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectile;
    Transform target;

    [SerializeField] float fireRate = 2f;
    [SerializeField] float searchSpeed = 40f;
    [SerializeField] float trackSpeed = 100f;
    [SerializeField] float detectionRange = 20f;
    [SerializeField] AttackController attackController;

    float fireTimer = 0f;

    ProjectileEnemyState projectileEnemyState;

    bool returning;
    float returnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        projectileEnemyState = ProjectileEnemyState.Searching;
        GetComponent<SphereCollider>().radius = detectionRange;

        Search();
    }

    private void OnValidate() {
        GetComponent<SphereCollider>().radius = detectionRange;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;

        switch (projectileEnemyState)
        {
            case ProjectileEnemyState.Searching:
                Search();
                break;
            case ProjectileEnemyState.Tracking:
                Track();
                break;
        }

    }

    private void Track()
    {

        if (target == null)
        {
            projectileEnemyState = ProjectileEnemyState.Searching;
        }

        Vector3 direction = target.position - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        eyeball.rotation = Quaternion.Lerp(eyeball.rotation, toRotation, Time.deltaTime * trackSpeed);

        if (fireTimer > fireRate)
        {
            fireTimer = 0f;
            attackController.Attack();
        }
    }

    private void Search()
    {
        if (returning)
        {
            returnTime += Time.deltaTime;
            Quaternion lookRotation = Quaternion.LookRotation(transform.forward, transform.up);
            eyeball.rotation = Quaternion.Lerp(eyeball.rotation, lookRotation, 0.01f);

            if (returnTime > 1f)
            {
                returning = false;
            }

            return;
        }

        eyeball.Rotate(new Vector3(0f, searchSpeed * Time.deltaTime, 0f));
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), projectileEnemyState.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
            projectileEnemyState = ProjectileEnemyState.Tracking;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
            returning = true;
            returnTime = 0;
            projectileEnemyState = ProjectileEnemyState.Searching;
        }
    }

    
}
