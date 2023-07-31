using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] bool godMode = false;
    [SerializeField] public int hp, armour, maxHP, maxArmour;//{get; private set;}
    [Range(0f,1f)][SerializeField] float armourReductionPercentage;
    //[SerializeField] int min = 0;

   private void Start()
   {
        // Prevents hp > maxHP
        hp = Mathf.Clamp(hp, 0, maxHP);
        //Debug.Log("Start HP: " + hp);
   }

    public void Damage(int ammount, bool piercingDamage=false)
    {
        if(!godMode){
            if(!piercingDamage){ ammount = ArmourReduction(ammount); }
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
        hp = Mathf.Clamp(hp+ammount, 0, maxHP);
        //Debug.Log("Recieved " + ammount + " healing!");
    }

    public void AddArmour(int ammount)
    {
        armour = Mathf.Clamp(armour+ammount, 0, maxArmour);
    }

    private int ArmourReduction(int ammount)
    {
        int armourDamage = (int)Mathf.Ceil(ammount*armourReductionPercentage);
        int remainingDamage = ammount - armourDamage;
        Debug.Log("Health Damage: " + remainingDamage);
        if(ammount < 0){ ammount = 0; }

        armour -= armourDamage;
        if(armour < 0){ armour = 0; }

        return remainingDamage;
    }
}
