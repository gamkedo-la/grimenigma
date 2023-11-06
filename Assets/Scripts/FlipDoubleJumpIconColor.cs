using UnityEngine;

public class FlipDoubleJumpIconColor : ChangeUIImageColorOnEvent
{
    PlayerMovement pMovement;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject pBody = GameObject.Find("Player/Body");
        pMovement = pBody.GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        pMovement.onAirJumpAvailable += UpdateColor;
    }

    void OnDisable()
    {
        pMovement.onAirJumpAvailable -= UpdateColor;
    }
}
