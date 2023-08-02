using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] public ProjectileTypes type;
    [SerializeField] public int damage = 1;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float range = 5000f;

    [HideInInspector] public string ownerTag;

    float travelDistance;

    void OnEnable()
    {
        travelDistance = range;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        travelDistance -= speed * Time.deltaTime;
        if(travelDistance <= 0) { gameObject.SetActive(false); }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
        if(other.gameObject.tag != ownerTag){
            //Debug.Log(this.gameObject.name + " collided with object " + other.gameObject.name + " with tag of " + ownerTag);
            other.transform.gameObject.GetComponent<HealthController>()?.Damage(damage);
            this.gameObject.SetActive(false);
        }
    }
}
