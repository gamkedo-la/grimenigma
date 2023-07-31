using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] public int hp;//{get; private set;}
    [SerializeField] bool godMode = false;
    [SerializeField] int max = 1;
    //[SerializeField] int min = 0;

   private void Start()
   {
        // Prevents hp > max
        hp = Mathf.Clamp(hp, 0, max);
        //Debug.Log("Start HP: " + hp);
   }

    public void Damage(int ammount)
    {
        if(!godMode){
            hp -= ammount;
            //Debug.Log("Recieved " + ammount + " damage!");

            if(hp < 1){
                //Debug.Log("I am dead!");
                UnityEngine.Object.Destroy(this.gameObject);
            }
        }
        else{ Debug.Log("Reminder! God mode is enabled."); }
    }

    public void Heal(int ammount)
    {
        hp = Mathf.Clamp(hp+ammount, 0, max);
        //Debug.Log("Recieved " + ammount + " healing!");
    }
}
