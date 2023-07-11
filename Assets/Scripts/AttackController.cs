using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTypes{
    Automatic,
    Burst,
    Melee,
    Hitscan,
    Explosive
}

public class AttackController : MonoBehaviour
{
    [SerializeField] AttackTypes attackType;
    [SerializeField] int hitScanDamage = 1;
    [SerializeField] float range = 500;
    [SerializeField] float cooldown = 0.2f;
    [SerializeField] float coneDegrees = 0f;
    [SerializeField] float patternSteps = 0f;
    [SerializeField] int projectileAmmount = 1;
    [SerializeField] PlayerCameraControl pCamera;

    Object projectilePrefab;
    Object clonedProjectile;
    RaycastHit attackHit;
    bool shouldAttack = true;

    public void Attack()
    {
        if(shouldAttack){
            Debug.Log("Attacking!");
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
        Debug.Log("Firing projectile!");
        clonedProjectile = Instantiate(projectilePrefab, pCamera.transform.position, pCamera.transform.rotation);
    }

    void FireHitscan()
    {
        Debug.Log("Firing hitscan!");
        Physics.Raycast(pCamera.transform.position, pCamera.transform.forward, out attackHit, range);
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
