using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public AttackController CurrentWeapon;
    public void Attack()
    {
        CurrentWeapon.Attack();
    }
}
