using UnityEngine;

public class DeathController: MonoBehaviour
{
    [Header("OnDeath")]
    [SerializeField] bool destroyOnDeath = true;
    [Header("Optional Settings")]
    [SerializeField] GameObject owner;

    public GameObject[] itemDrops;
    int randomItemDrop;
    RaycastHit hit;
    GameObject thingToKill;
    public void HandleDeath()
    {
        if(this.gameObject.tag == "enemy") {
            randomItemDrop = Random.Range(0, itemDrops.Length);
            if (Physics.Raycast(this.gameObject.transform.position + new Vector3(0, 4, 0), Vector3.down, out hit))
            {
                // Calculate the item drop position based on the ground hit point
                Vector3 groundHitPoint = hit.point;

                // Set the item drop position at a fixed height above the ground
                Vector3 itemDropPosition = new Vector3(groundHitPoint.x, groundHitPoint.y + 1f, groundHitPoint.z);

                Instantiate(itemDrops[randomItemDrop], itemDropPosition, this.gameObject.transform.rotation);
            }
        }

        if(destroyOnDeath){ Destroy(thingToKill); }
        else { thingToKill.SetActive(false); }

        //Debug.Log("Entity " + this + " has died!");
    }

    void Start(){
        if(owner != null){ thingToKill = owner; }
        else{ thingToKill = this.gameObject; }
    }
}
