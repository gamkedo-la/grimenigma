using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] AttackTypes attackType;
    [SerializeField] bool piercingDamage;
    [SerializeField] int hitScanDamage = 1;
    [SerializeField] public float range;
    [SerializeField] float cooldown, spread, drawTime;
    [Header("For Crosshar Shooting")]
    [SerializeField] bool hasSourceOfTruth;
    [SerializeField] GameObject sourceOfTruth;
    [Header("Projectile")]
    [SerializeField] GameObject projectile;
    [Header("Tracer")]
    [SerializeField] bool shouldRenderTracer;
    [SerializeField] float tracerLifeSpan;
    [SerializeField] LineRenderer tracerRenderer;
    //[SerializeField] float patternSteps = 0f;
    [Header("Ammo")]
    [SerializeField] int projectileAmmount = 1;
    [SerializeField] bool infiniteAmmmo;
    [SerializeField] int ammo, maxAmmo;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip fxSound;
    [Header("Bodge Settings")]
    [SerializeField] string ownerTag;

    Transform spawnOrigin;
    ProjectilePooler poolerSingleton;

    bool shouldAttack = true;
    Vector3 targetPosition;
    Object projectilePrefab, clonedProjectile;
    GameObject hitscanTracer;
    RaycastHit attackHit;

    public void Attack()
    {
        // Listen, checking the weapon every attack was the fastest way to implement this...
        if(ammo < 1 && !infiniteAmmmo){
            // Play no ammo sound on this line.
            return;
        }

        if(shouldAttack){
            PlaySoundFX();
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

    // Start is called before the first frame update
    void Start()
    {
        poolerSingleton = FindObjectOfType<ProjectilePooler>().gameObject.GetComponent<ProjectilePooler>();
        tracerRenderer = GetComponent<LineRenderer>();
        if(!hasSourceOfTruth){ sourceOfTruth = this.gameObject; }

        spawnOrigin = this.gameObject.transform;

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

    void Update()
    {
        if(hasSourceOfTruth){ transform.LookAt(sourceOfTruth.transform ); }
    }

    void DrawTracer()
    {
        targetPosition = spawnOrigin.position + spawnOrigin.forward * range;

        tracerRenderer.SetPosition(0, transform.position);
        tracerRenderer.SetPosition(1, targetPosition);
    }

    Vector3 GetDirection()
    {
        //targetPosition = spawnOrigin.position + spawnOrigin.forward * range;
        targetPosition = sourceOfTruth.transform.position + sourceOfTruth.transform.forward *500f;
        targetPosition = new Vector3(
                                            targetPosition.x + Random.Range(-spread, spread),
                                            targetPosition.y + Random.Range(-spread, spread),
                                            targetPosition.z + Random.Range(-spread, spread)
                                            );

        
        return (targetPosition - spawnOrigin.position).normalized;
    }

    void PlaySoundFX()
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(fxSound);
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
            if(shouldRenderTracer){ StartCoroutine(RunCreateAndDestroyTracer()); }

        }
    }

    IEnumerator RunFireHitscan()
    {
        //Debug.Log("Firing hitscan!");
        yield return new WaitForSeconds(drawTime);

        Physics.Raycast(spawnOrigin.position, GetDirection(), out attackHit, range);
        if(shouldRenderTracer){ StartCoroutine(RunCreateAndDestroyTracer()); }
        Debug.Log(attackHit.collider);
        attackHit.transform.gameObject.GetComponent<HealthController>()?.Damage(hitScanDamage, piercingDamage);
    }

    IEnumerator RunResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(cooldown);
        shouldAttack = true;
    }

    IEnumerator RunCreateAndDestroyTracer()
    {
        targetPosition = spawnOrigin.position + spawnOrigin.forward * range;
        tracerRenderer.SetPosition(0, transform.position);
        tracerRenderer.SetPosition(1, targetPosition);

        tracerRenderer.enabled = true;
        yield return new WaitForSeconds(tracerLifeSpan);
        tracerRenderer.enabled = false;
    }
}
