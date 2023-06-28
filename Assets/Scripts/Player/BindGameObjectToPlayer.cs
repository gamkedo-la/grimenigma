using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindGameObjectToPlayer : MonoBehaviour
{
    [Header("Game Object Dependencies")]
    [SerializeField] Transform player;

    // Update is called once per frame
    void Update(){
        // Move to player position.
        transform.position = new Vector3(player.position.x, (player.position.y), player.position.z);

    }
}
