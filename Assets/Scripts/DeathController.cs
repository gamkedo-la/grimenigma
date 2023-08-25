using UnityEngine;

public class DeathController: MonoBehaviour
{
    public void HandleDeath(bool shouldDestory)
    {
        if(shouldDestory){ Destroy(this.gameObject); }
        else { this.gameObject.SetActive(false); }
    }
}
