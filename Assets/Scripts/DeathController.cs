using UnityEngine;

public class DeathController: MonoBehaviour
{
    [Header("OnDeath")]
    [SerializeField] bool destroyOnDeath = true;
    [SerializeField] float delay = 0;
    [SerializeField] bool dropItem;
    [SerializeField] public GameObject[] itemDrops;
    [Header("Optional Settings")]
    [SerializeField] GameObject owner;

    public event System.Action<GameObject> onDeath;

    GameObject thingToKill;

    public void HandleDeath()
    {
        onDeath?.Invoke(gameObject);
        if(dropItem){ DropResource(); }
        if(destroyOnDeath){ Destroy(thingToKill, delay); }
        else { thingToKill.SetActive(false); }

        //Debug.Log("Entity " + this + " has died!");
    }

    void Start(){
        if(owner != null){ thingToKill = owner; }
        else{ thingToKill = this.gameObject; }
    }

    void DropResource()
    {
            int randomItemDrop = Random.Range(0, itemDrops.Length);
            RaycastHit hit;
            
            if (Physics.Raycast(this.gameObject.transform.position + new Vector3(0, 4, 0), Vector3.down, out hit))
            {
                // Calculate the item drop position based on the ground hit point
                Vector3 groundHitPoint = hit.point;

                // Set the item drop position at a fixed height above the ground
                Vector3 itemDropPosition = new Vector3(groundHitPoint.x, groundHitPoint.y + 1f, groundHitPoint.z);

                Instantiate(itemDrops[randomItemDrop], itemDropPosition, this.gameObject.transform.rotation);
            }
    }
}
