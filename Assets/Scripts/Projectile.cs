using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 1f;

    [HideInInspector] public float range = 5000f;
    [HideInInspector] public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        range -= speed;
        if(range <= 0) { UnityEngine.Object.Destroy(this.gameObject); }
    }
}
