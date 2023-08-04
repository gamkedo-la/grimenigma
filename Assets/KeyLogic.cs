using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScriptedAnimations))]
public class KeyLogic : MonoBehaviour
{
    [Header("Key Stuff")]
    [SerializeField] string label;
    [Header("Animation")]
    [SerializeField] ScriptedAnimations sa;
    [Header("Twean")]
    [SerializeField] float distanceX;
    [SerializeField] float distanceY;
    [SerializeField] float distanceZ;
    [SerializeField] float speed;
    [Header("Rotation")]
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;


    void Start()
    {

    }

    void OnEnable()
    {
        sa.Bob(speed, distanceX, distanceY, distanceZ);
        sa.Rotate(rotateX, rotateY, rotateZ);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
