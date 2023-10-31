using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandlePlayerDeath : MonoBehaviour
{
    [SerializeField] DeathController pDeathController;

    void OnEnable()
    {
        pDeathController.onDeath += HandleDeath;
    }

    void OnDisable()
    {
        pDeathController.onDeath -= HandleDeath;
    }

    void HandleDeath(GameObject deadActor)
    {
        SceneManager.LoadScene(1);
    }

}
