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
        pMovement.onAirJumpAvailable += UpdateDoubleJumpColor;
    }

    // Update is called once per frame
    void UpdateDoubleJumpColor(bool isAvailable)
    {
        switch (isAvailable)
        {
            case true:
                SetColor(baseColor);
                break;
            default:
                SetColor(altColor);
                break;
        }
    }
}
