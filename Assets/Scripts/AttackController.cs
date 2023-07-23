using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] AttackTypes attackType;
    [SerializeField] GameObject projectile;
    [SerializeField] int hitScanDamage = 1;
    [SerializeField] float range = 500;
    [SerializeField] float cooldown = 0.2f;
    [SerializeField] float spread = 0f;
    [SerializeField] float patternSteps = 0f;
    [SerializeField] int ammount = 1;
    [SerializeField] string ownerTag;
    [SerializeField] GameObject spawnOrigin;

    ProjectilePooler poolerSingleton;
    Object projectilePrefab;
    Object clonedProjectile;
    RaycastHit attackHit;
    bool shouldAttack = true;
    Vector3 position;

    public void Attack()
    {
        // Listen, checking the weapon every attack was the fastest way to implement this...
            if(shouldAttack){
                //Debug.Log("Attacking!");
                switch (attackType)
                {
                    case AttackTypes.Automatic:
                        FireProtectile();
                        break;
                    case AttackTypes.Hitscan:
                        FireHitscan();
                        break;
                    default:
                        Debug.LogError("Invalid Projectile Value!");
                        break;
                }

            StartCoroutine(RunResetAttackCooldown());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        poolerSingleton = FindObjectOfType<ProjectilePooler>().gameObject.GetComponent<ProjectilePooler>();

        switch (attackType)
        {
            case AttackTypes.Automatic:
                projectilePrefab = Resources.Load("Prefabs/Projectiles/Projectile", typeof(GameObject));
                break;
            case AttackTypes.Hitscan:
                break;
            default:
                Debug.LogError("Invalid Projectile Value!");
                break;
        }
        
    }

    void FireProtectile()
    {
        //Debug.Log("Firing projectile!");

        for (int i = 0; i < ammount; i++)
        {
            position = new Vector3(
                        spawnOrigin.transform.position.x + Random.Range(-spread, spread),
                        spawnOrigin.transform.position.y + Random.Range(-spread, spread),
                        spawnOrigin.transform.position.z
            );
            //Debug.Log("Instatiating projectile!");
            GameObject rentedProjectile = poolerSingleton.GetObjectFromPool(projectile);
            rentedProjectile.GetComponent<Projectile>().ownerTag = ownerTag;
            rentedProjectile.transform.position = position;
            rentedProjectile.transform.rotation = spawnOrigin.transform.rotation;
            rentedProjectile.gameObject.SetActive(true);

        }
    }

    void FireHitscan()
    {
        //Debug.Log("Firing hitscan!");
        position = new Vector3(
                                spawnOrigin.transform.position.x + Random.Range(-spread, spread),
                                spawnOrigin.transform.position.y + Random.Range(-spread, spread),
                                spawnOrigin.transform.position.z
        );
        Physics.Raycast(position, spawnOrigin.transform.forward, out attackHit, range);
        attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(hitScanDamage);
    }

    IEnumerator RunResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(cooldown);
        shouldAttack = true;
    }
}
