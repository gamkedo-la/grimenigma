using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemyAI : MonoBehaviour
{

    [SerializeField] AttackController attackController;
    EnemyVision enemyVision;

    private void Awake() {
        enemyVision = GetComponent<EnemyVision>();
    }

    void Update()
    {

       if(enemyVision.canSeeTarget)
        {
            attackController.Attack();
        }

    }

}
