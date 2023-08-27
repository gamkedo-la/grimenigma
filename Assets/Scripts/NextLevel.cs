using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] Collider myCollider;
    // Start is called before the first frame update
    void Start()
    {
        if(myCollider == null) { myCollider = GetComponent<Collider>(); }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        //Debug.Log("Trigger collision with " + other.gameObject.name);
        if(other.tag == "Player")
            SceneManager.LoadScene("Scenes/LevelEnd", LoadSceneMode.Single);
        }
}
