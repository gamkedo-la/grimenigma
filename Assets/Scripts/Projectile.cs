using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float speed = 1f;
    [SerializeField] float range = 5000f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        range -= speed * Time.deltaTime;
        if(range <= 0) { UnityEngine.Object.Destroy(this.gameObject); }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Projectile collision!");
        other.transform.gameObject.GetComponent<HealthController>()?.Damage(damage);
        this.gameObject.SetActive(false);
    }
}
