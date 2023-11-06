using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] bool spawnOnEnable;
    [SerializeField] bool disableOnSpawn;
    [SerializeField] bool spawnAlerted;
    [SerializeField] GameObject thingToSpawn;

    public event System.Action<string, GameObject> OnSpawn;

    EncounterListener listener;
    GameObject spawnedEnemy;

    void Start()
    {
        listener = GetComponent<EncounterListener>();
        listener.onEvent += TriggerSpawn;

        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    void OnEnable()
    {
        if(spawnOnEnable){ TriggerSpawn("OnEnable"); }
    }

    public void TriggerSpawn(string label)
    {
        spawnedEnemy = Instantiate(thingToSpawn, transform.position, transform.rotation);
        if(!spawnOnEnable){ OnSpawn.Invoke(label, spawnedEnemy); }
        if(disableOnSpawn){ this.gameObject.SetActive(false); }
        if(spawnAlerted){ SetAlerted(); }
    }

    void SetAlerted()
    {
        EnemyBaseAI component;
        if((component = spawnedEnemy.GetComponent<EnemyBaseAI>()) != null){
            component.state = AIState.alerted;
    }
    }
}