using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] public int hp{get; private set;}
    [SerializeField] int max = 1;
    [SerializeField] int min = 0;

   private HealthController()
   {
        // Prevents hp > max
        hp = Mathf.Clamp(hp, min, max);
   }

    public void Damage(int ammount)
    {
        hp -= ammount;
        Debug.Log("Recieved " + ammount + " damage!");

        if(hp < 1){
            //Debug.Log("I am dead!");
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }

    public void Heal(int ammount)
    {
        hp = Mathf.Clamp(hp+ammount, min, max);
        Debug.Log("Recieved " + ammount + " healing!");
    }
}
