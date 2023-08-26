using UnityEngine;

public class DeathController: MonoBehaviour
{
    [Header("Optional Settings")]
    [SerializeField] GameObject owner;

    GameObject thingToKill;
    public void HandleDeath(bool shouldDestory)
    {
        if(owner != null){ thingToKill = owner; }
        else{ thingToKill = this.gameObject; }

        if(shouldDestory){ Destroy(thingToKill); }
        else { thingToKill.SetActive(false); }

        Debug.Log("Entity " + this + " has died!");
    }
}
