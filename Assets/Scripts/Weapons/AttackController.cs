using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum AttackTypes{
        Projectile,
        Hitscan,
    }

public class AttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] AttackTypes attackType;
    [SerializeField] bool piercingDamage;
    [SerializeField] int hitScanDamage = 1;
    [SerializeField] public float range, cooldown, spread, drawTime;
    [Header("For Crosshair Shooting")]
    [SerializeField] bool hasCrosshair;
    [SerializeField] GameObject sourceOfTruth;
    [Header("Projectile")]
    [SerializeField] int poolID = 0;
    [SerializeField] GameObject projectile;
    [SerializeField] bool setProjectileLayer;
    [Range(0,32)] [SerializeField] int projectileLayer;
    [Header("Tracer")]
    [SerializeField] bool shouldRenderTracer;
    [SerializeField] GameObject tracer;
    [SerializeField] float tracerLifeSpan;
    //[SerializeField] float patternSteps = 0f;
    [Header("Ammo")]
    [SerializeField] int projectileAmmount = 1;
    [SerializeField] public bool infiniteAmmmo;
    [SerializeField] public int ammo, maxAmmo;
    [Header("Audio")]
    [SerializeField] AudioSource soundSource;
    [SerializeField] AudioClip firingSFX;
    [SerializeField] bool hasChargeSound;
    [SerializeField] AudioClip chargeSFX;

    [Header("Bodge Settings")]
    [SerializeField] bool bodge_isEnemy = false;
    [SerializeField] string ownerTag;
    [SerializeField] LayerMask inclusionMasks;
    [SerializeField] Transform customSpawnOrigin;

    public event System.Action onCharging;
    public event System.Action onAttack;
    public event System.Action onAttackStep;

    Transform spawnOrigin;
    ProjectilePooler poolerSingleton;
    LineRenderer tracerRenderer;

    bool shouldAttack = true;
    float targetRange;
    Vector3 targetPosition;
    Object projectilePrefab, clonedProjectile;
    GameObject hitscanTracer;
    RaycastHit attackHit, pointingAt;
    Transform player;

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
                case AttackTypes.Projectile:
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
        //Debug.Log(projectileLayer);
        poolerSingleton = FindPoolerWithId(poolID);

        if (!poolerSingleton) { Debug.LogError("ERROR: AttackController was unable to find a ProjectilePooler!"); }

        if(attackType == AttackTypes.Hitscan && shouldRenderTracer){
            tracerRenderer = this.AddComponent<LineRenderer>();
            tracer = Instantiate(tracer, parent:this.gameObject.transform);
            tracerRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
        if(!hasCrosshair){ sourceOfTruth = this.gameObject; }

        spawnOrigin = customSpawnOrigin == null ? this.gameObject.transform : customSpawnOrigin;
        //Debug.Log(spawnOrigin);

        if(bodge_isEnemy){ player = GameObject.Find("Player/Body").transform; }
    }

    void Update()
    {
        if(hasCrosshair){ transform.LookAt(sourceOfTruth.transform ); }
    }

    void OnDestroy()
    {
        if(attackType == AttackTypes.Hitscan
            && shouldRenderTracer
            && this.tracerRenderer!=null
            && this.tracerRenderer.material!=null)
        {
            Destroy(this.tracerRenderer.material);
        }
    }

    void DrawTracer()
    {
        targetPosition = spawnOrigin.position + spawnOrigin.forward * range;

        tracerRenderer.SetPosition(0, transform.position);
        tracerRenderer.SetPosition(1, targetPosition);
    }

    ProjectilePooler FindPoolerWithId(int targetId)
    {
        ProjectilePooler[] poolers = FindObjectsOfType<ProjectilePooler>();

        foreach (ProjectilePooler pooler in poolers){
            if (pooler.id == targetId) { return pooler; }
        }

        return null;
    }

    Vector3 GetDirection()
    {
        targetRange = range;

        // Shotguns were breaking when aiming at enemies. Hense the projectileAmmount check.
        if(projectileAmmount == 1 && Physics.Raycast(sourceOfTruth.transform.position, sourceOfTruth.transform.forward, out pointingAt, range, inclusionMasks)){
            if(pointingAt.transform.gameObject.TryGetComponent<HealthController>(out var component)){
                targetRange = Vector3.Distance(sourceOfTruth.transform.position, pointingAt.transform.position);
            }
        }

        //Debug.Log("targetRange:" + targetRange);

        targetPosition = sourceOfTruth.transform.position + sourceOfTruth.transform.forward * targetRange;
        targetPosition = new Vector3(
                                    targetPosition.x + Random.Range(-spread, spread),
                                    targetPosition.y + Random.Range(-spread, spread),
                                    targetPosition.z + Random.Range(-spread, spread)
                                    );

        if(bodge_isEnemy){
            Debug.DrawLine(transform.position, (targetPosition - spawnOrigin.position), Color.red, 1);
            //targetPosition = sourceOfTruth.transform.forward;
            //return targetPosition;
        }

        return targetPosition - spawnOrigin.position;
    }


    void PlaySoundFX(AudioClip sound)
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(sound);
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj){ return; }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform){
            if (null == child) { continue; }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    IEnumerator RunFireProtectile()
    {
        //Debug.Log("Firing projectile!");
        onCharging?.Invoke();
        if(hasChargeSound){ PlaySoundFX(chargeSFX); }
        yield return new WaitForSeconds(drawTime);
        onAttack?.Invoke();
        PlaySoundFX(firingSFX);
        for (int i = 0; i < projectileAmmount; i++)
        {
            onAttackStep?.Invoke();
            //Debug.Log("Instatiating projectile!");
            GameObject rentedProjectile = poolerSingleton.GetObjectFromPool(projectile);
            // Don't call GetComponent twice like this. TO DO: fix that.
            rentedProjectile.GetComponent<Projectile>().owner = gameObject;
            rentedProjectile.GetComponent<Projectile>().ownerTag = ownerTag;
            if(setProjectileLayer){ SetLayerRecursively(rentedProjectile, projectileLayer); }
            rentedProjectile.transform.position = spawnOrigin.position;
            rentedProjectile.transform.rotation = Quaternion.LookRotation(GetDirection());
            if(bodge_isEnemy){ rentedProjectile.transform.LookAt(player.transform); }
            rentedProjectile.gameObject.SetActive(true);
            if(shouldRenderTracer){ StartCoroutine(RunCreateAndDestroyTracer(range)); }
            onAttackStep?.Invoke();
        }
    }

    IEnumerator RunFireHitscan()
    {
        //Debug.Log("Firing hitscan!");
        onCharging?.Invoke();
        if(hasChargeSound){ PlaySoundFX(chargeSFX); }
        yield return new WaitForSeconds(drawTime);
        onAttack?.Invoke();

        float distance = range;

        if(Physics.Raycast(spawnOrigin.position, GetDirection(), out attackHit, range)){
            distance = Vector3.Distance(transform.position, attackHit.transform.position);
            if(attackHit.transform.gameObject.TryGetComponent<HealthController>(out var component)){
                component.Damage(hitScanDamage, gameObject, piercingDamage);
            }
        }

        //Physics.Raycast(spawnOrigin.position, GetDirection(), out attackHit, range);
        if(shouldRenderTracer){
            StartCoroutine(RunCreateAndDestroyTracer(distance));
        }
        //Debug.Log(attackHit.collider);

        //attackHit.transform.gameObject?.GetComponent<HealthController>().Damage(hitScanDamage, piercingDamage);
    }

    IEnumerator RunResetAttackCooldown()
    {
        //Debug.Log("Started attack cooldown!");
        shouldAttack = false;
        yield return new WaitForSeconds(cooldown);
        shouldAttack = true;
    }

    IEnumerator RunCreateAndDestroyTracer(float distance)
    {
        targetPosition = spawnOrigin.position + spawnOrigin.forward * distance;

        tracerRenderer.startColor = Color.red;
        tracerRenderer.endColor = Color.white;
        tracerRenderer.SetPosition(0, transform.position);
        tracerRenderer.SetPosition(1, targetPosition);

        tracerRenderer.enabled = true;
        yield return new WaitForSeconds(tracerLifeSpan);
        tracerRenderer.enabled = false;
    }
}
