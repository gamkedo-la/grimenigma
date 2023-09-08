using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ScriptedAnimations))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] public ProjectileTypes type;
    [SerializeField] public int damage = 1;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float range = 5000f;
    [Header("Explosive Properties")]
    [SerializeField] float exposionRadius;
    [SerializeField] int maxHits;
    [SerializeField] LayerMask masksToHit;
    [SerializeField] LayerMask blockExplosionMasks;
    [Header("Animaton")]
    [SerializeField] ScriptedAnimations sa;
    [SerializeField] float rotateX;
    [SerializeField] float rotateY;
    [SerializeField] float rotateZ;

    [HideInInspector] public string ownerTag;

    float travelDistance;
    Vector3 direction;
    Collider[] explosiveHits;

    void Start()
    {
        if(type == ProjectileTypes.Explosive){ explosiveHits = new Collider[maxHits]; }
    }

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
        if(other.gameObject.tag == "ignoreTrigger"){
            return;
        }
        //Debug.Log(this.gameObject.name + " collided with " + other.gameObject.name);
        if(other.gameObject.tag != ownerTag){
            //Debug.Log(this.gameObject.name + " with ownerTag of " + ownerTag + " collided with object " + other.gameObject.name);
            HandleCollision(other.gameObject);
            gameObject.SetActive(false);
        }
    }

    void HandleCollision(GameObject target)
    {
        switch (type)
        {
            case ProjectileTypes.Normal:
                target.GetComponent<HealthController>()?.Damage(damage);
                break;
            case ProjectileTypes.Explosive:
                SpawnExplosion();
                break;
            default:
                Debug.LogError("No case to handle damage for pojectile type of " + type + "!");
                break;

        }
    }

    void SpawnExplosion()
    {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, exposionRadius, explosiveHits, masksToHit);
        for(int i = 0; i < hits; i++){
            //Debug.Log(explosiveHits[i]);
            if(explosiveHits[i].TryGetComponent<Rigidbody>( out Rigidbody rb)){
                float distance = Vector3.Distance(transform.position, explosiveHits[i].transform.position);
                if(Physics.Raycast(transform.position, (explosiveHits[i].transform.position - transform.position).normalized, distance, blockExplosionMasks)){
                    //Debug.Log("Expolsion can reach!");
                    explosiveHits[i].GetComponent<HealthController>()?.Damage(damage);
                }
            }
        }
    }
}
