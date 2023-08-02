using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    [SerializeField] public float speed, minSpeed, maxSpeed;

    public void ChangeSpeed(float ammount)
    {
        speed = Mathf.Clamp(speed+ammount, minSpeed, maxSpeed);
    }
}
