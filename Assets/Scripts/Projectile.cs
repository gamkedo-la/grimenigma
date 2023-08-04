using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ScriptedAnimations))]
public class Projectile : MonoBehaviour
{
    [SerializeField] public ProjectileTypes type;
    [SerializeField] public int damage = 1;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float range = 5000f;
    [Header("Animaton")]
    [SerializeField] ScriptedAnimations sa;
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;

    [HideInInspector] public string ownerTag;

    float travelDistance;
    Vector3 direction;

    void OnEnable()
    {
        travelDistance = range;
        direction = transform.forward;
        sa.Rotate(rotateX, rotateY, rotateZ);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        travelDistance -= speed * Time.deltaTime;
        if(travelDistance <= 0) { gameObject.SetActive(false); }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
        if(other.gameObject.tag != ownerTag){
            //Debug.Log(this.gameObject.name + " with ownerTag of " + ownerTag + " collided with object " + other.gameObject.name);
            other.transform.gameObject.GetComponent<HealthController>()?.Damage(damage);
            this.gameObject.SetActive(false);
        }
    }
}
