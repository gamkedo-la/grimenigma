using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAI : MonoBehaviour
{

    enum ProjectileEnemyState { Searching, Tracking }

    [SerializeField] Transform eyeball;
    [SerializeField] Transform firePoint;
    [SerializeField] Projectile projectile;
    Transform player;

    [SerializeField] float fireRate = 2f;
    [SerializeField] float detectionRange = 75f;
    [SerializeField] float detectionAngleDegrees = 40f;
    [SerializeField] float searchSpeed = 40f;
    [SerializeField] float trackSpeed = 100f;

    float fireTimer = 0f;

    ProjectileEnemyState projectileEnemyState;

    Vector3 eyeballLookDirection;
    Vector3 playerDirection;
    float angleToPlayer;
    float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        projectileEnemyState = ProjectileEnemyState.Searching;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        Search();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        UpdatePlayerTracking();

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

    void UpdatePlayerTracking()
    {
        eyeballLookDirection = firePoint.forward;
        eyeballLookDirection.y = 0;
        playerDirection = (player.position - eyeball.position) + player.up * 1.5f;
        playerDirection.y = 0;

        angleToPlayer = Vector3.Angle(eyeballLookDirection, playerDirection);
        distanceToPlayer = Vector3.Distance(eyeball.position, player.position);
    }

    private void Track()
    {
        
        if (angleToPlayer > detectionAngleDegrees)
        {
            projectileEnemyState = ProjectileEnemyState.Searching;
        }

        if (angleToPlayer > detectionRange)
        {
            return;
        }

        eyeball.rotation = Quaternion.RotateTowards(eyeball.rotation, player.rotation,trackSpeed * Time.deltaTime);

        if (fireTimer > fireRate)
        {
            fireTimer = 0f;
            Instantiate(projectile, firePoint.position, Quaternion.identity);
        }
    }

    private void Search()
    {
        if (angleToPlayer < detectionAngleDegrees)
        {
            projectileEnemyState = ProjectileEnemyState.Tracking;
        }

        eyeball.Rotate(new Vector3(0f, searchSpeed * Time.deltaTime, 0f));

    }

    /* private void OnDrawGizmos() {
        Debug.DrawLine(eyeball.position, eyeball.position + eyeballLookDirection, Color.red, 0.10f);
        Debug.DrawLine(eyeball.position, playerDirection, Color.blue, 0.10f);
    } */


    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 50), projectileEnemyState.ToString() + " : " + angleToPlayer + " : " + distanceToPlayer);
    }
}
