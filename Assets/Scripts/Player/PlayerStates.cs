using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [HideInInspector] public bool hasLandedThisCycle, isGrounded, isSliding;
    [HideInInspector] public Vector3 landingVelocity;
}
