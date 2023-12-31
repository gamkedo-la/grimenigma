using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooler : MonoBehaviour
{
    [SerializeField] public int id = 0;
    [SerializeField] private List<GameObject> _bulletPrefabs = new List<GameObject>();
    private Dictionary<GameObject, List<GameObject>> _allPools;
    private int _totalObjects = 0;

    public int TotalBullets { get => _totalObjects; }

    private void Start()
    {
        _allPools = new Dictionary<GameObject, List<GameObject>>();
        // Speeds up the game to prefill the dictionary with the bullet types that will be used.
        foreach (var pre in _bulletPrefabs) { AddNewBulletTypeToPool(pre); }
    }

    public GameObject GetObjectFromPool(GameObject type)
    {
        foreach (var pair in _allPools) // Should consider chaning this to if(_allPools.Contains(_type)).
        {
            var prefab = pair.Key;
            var currPool = pair.Value;

            if (prefab == type){
                for (int i = 0; i < currPool.Count; i++) {
                    if (!currPool[i].activeInHierarchy) {
                        //Debug.Log("Current pooled object: " + currPool[i]);
                        return currPool[i];
                    }
                }

                return MakeNewGameObject(prefab);
            }
        }
        Debug.LogError("Uh oh spaghettios. Bullet type " + type + " does not exist as a pool!");
        return null;
    }

    private void AddNewBulletTypeToPool(GameObject prefab)
    {
        if (!_allPools.ContainsKey(prefab)) {
            _allPools.Add(prefab, new List<GameObject>());
            GameObject InstantiatedGameObject = Instantiate(prefab, parent:this.transform);
            InstantiatedGameObject.SetActive(false);
            _allPools[prefab].Add(InstantiatedGameObject);
            _totalObjects++;
        }
    }

    private GameObject MakeNewGameObject(GameObject newGameObject)
    {
        GameObject InstantiatedGameObject = Instantiate(newGameObject, parent:this.transform);
        InstantiatedGameObject.SetActive(false);
        _allPools[newGameObject].Add(InstantiatedGameObject);
        _totalObjects++;
        return InstantiatedGameObject;
    }
}