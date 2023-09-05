using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    [Range(0f, 100f)][SerializeField] public float hardLandingThreshold;
    [HideInInspector] public bool hasLandedThisCycle, isGrounded, isSliding;
    [HideInInspector] public Vector3 landingVelocity;
    [HideInInspector] public GameObject leftItem, rightItem;
}
