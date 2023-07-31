using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField] string weaponName;
    [SerializeField] AttackTypes attackType;
    [SerializeField] GameObject projectile;
    [SerializeField] bool piercingDamage;
    [SerializeField] int hitScanDamage = 1;
    [SerializeField] float range, cooldown, spread, drawTime;
    //[SerializeField] float patternSteps = 0f;
    [SerializeField] int projectileAmmount = 1;
    [SerializeField] bool infiniteAmmmo;
    [SerializeField] int ammo, maxAmmo;
    [SerializeField] string ownerTag;
    [SerializeField] Transform camera;

    Transform spawnOrigin;

    ProjectilePooler poolerSingleton;
    Object projectilePrefab;
    Object clonedProjectile;

    RaycastHit attackHit;
    bool shouldAttack = true;
    Vector3 position;

    public void Attack()
    {
        // Listen, checking the weapon every attack was the fastest way to implement this...
        if(ammo < 1 && !infiniteAmmmo){
            // Play no ammo sound on this line.
            return;
        }

        if(shouldAttack){
            //Debug.Log("Attacking!");
            switch (attackType)
            {
                case AttackTypes.Automatic:
                    StartCoroutine(RunFireProtectile());
                    break;
                case AttackTypes.Hitscan:
                    StartCoroutine(RunFireHitscan());
                    break;
                default:
                    Debug.LogError("Invalid Projectile Value!");
                    break;
            }
        
            if(!infiniteAmmmo){ ammo -= 1; }
            StartCoroutine(RunResetAttackCooldown());
        }
    }

    public void AddAmmo(int ammount)
    {
        ammo = Mathf.Clamp(ammo+ammount, 0, maxAmmo);
        //Debug.Log("Added " + ammount + " ammo to " + weaponName);
    }

    private void Awake()
    {
        spawnOrigin = this.gameObject.transform;
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

    Vector3 GetDirection()
    {
        Vector3 targetPosition = spawnOrigin.position + spawnOrigin.forward * range;
        targetPosition = new Vector3(
                                            targetPosition.x + Random.Range(-spread, spread),
                                            targetPosition.y + Random.Range(-spread, spread),
                                            targetPosition.z + Random.Range(-spread, spread)
                                            );

        
        return (targetPosition - spawnOrigin.position).normalized;
    }

    IEnumerator RunFireProtectile()
    {
        //Debug.Log("Firing projectile!");
        yield return new WaitForSeconds(drawTime);
        for (int i = 0; i < projectileAmmount; i++)
        {
            //Debug.Log("Instatiating projectile!");
            GameObject rentedProjectile = poolerSingleton.GetObjectFromPool(projectile);
            rentedProjectile.GetComponent<Projectile>().ownerTag = ownerTag;
            rentedProjectile.transform.position = spawnOrigin.position;
            rentedProjectile.transform.rotation = Quaternion.LookRotation(GetDirection());
            rentedProjectile.gameObject.SetActive(true);

        }
    }

    IEnumerator RunFireHitscan()
    {
        //Debug.Log("Firing hitscan!");
        yield return new WaitForSeconds(drawTime);

        Physics.Raycast(spawnOrigin.position, GetDirection(), out attackHit, range);
        attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(hitScanDamage, piercingDamage);
    }

    IEnumerator RunResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(cooldown);
        shouldAttack = true;
    }
}
