using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDashIconColor : ChangeUIImageColorOnEvent
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
        pMovement.onDashAvailable += UpdateColor;
    }

    void OnDisable()
    {
        pMovement.onDashAvailable -= UpdateColor;
    }

}
