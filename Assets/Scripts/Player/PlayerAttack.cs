using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Rail CurrentWeapon;
    public void Attack()
    {
        CurrentWeapon.Attack();
    }
}
