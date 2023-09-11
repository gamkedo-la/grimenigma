using UnityEngine;

[RequireComponent(typeof(DeathController))]
public class HealthController: MonoBehaviour
{
    [Header("Health Controller")]
    [SerializeField] bool godMode = false;
    [SerializeField] public int baseHP, maxHP, armour, maxArmour;//{get; private set;}
    [Range(0f,1f)][SerializeField] float armourReductionPercentage;
    [Header("Audio")]
    [SerializeField] bool hasAudioFX;
    [SerializeField] AudioClip onDamageSound;

    [HideInInspector] public int hp;

    DeathController deathController;
    public UIController UIController;

   private void Start()
   {
        deathController = GetComponent<DeathController>();
        //UIController = GetComponent<UIController>();

        // Prevents hp > maxHP
        hp = Mathf.Clamp(hp, 0, maxHP);
        UIController.SetHealth(hp);
        //Debug.Log("Start HP: " + hp);
   }
    AudioSource soundSource;

    public void Damage(int ammount, bool piercingDamage=false)
    {
        if(hasAudioFX){ PlaySoundFX(onDamageSound); }
        if(!godMode){
            if(!piercingDamage){ ammount = ArmourReduction(ammount); }
            hp -= ammount;
            //Debug.Log("Recieved " + ammount + " damage!");

            if(hp < 1){
                //Debug.Log("I am dead!");
                deathController.HandleDeath();
            }
            UIController.SetHealth(hp);
        }
        else{ Debug.Log("Reminder! God mode is enabled."); }
    }

    public void Heal(int ammount)
    {
        hp = Mathf.Clamp(hp+ammount, 0, maxHP);
        UIController.SetHealth(hp);
        //Debug.Log("Recieved " + ammount + " healing!");
    }

    public void AddArmour(int ammount)
    {
        armour = Mathf.Clamp(armour+ammount, 0, maxArmour);
    }


   private void Start()
   {
        deathController = GetComponent<DeathController>();
        if(hasAudioFX && !TryGetComponent<AudioSource>(out soundSource)){
            soundSource = gameObject.AddComponent<AudioSource>();
        }

        // Prevents hp > maxHP
        hp = Mathf.Clamp(baseHP, 0, maxHP);
        //Debug.Log("Start HP: " + hp);
   }
    private int ArmourReduction(int ammount)
    {
        int armourDamage = (int)Mathf.Ceil(ammount*armourReductionPercentage);
        int remainingDamage = ammount - armourDamage;
        //Debug.Log("Health Damage: " + remainingDamage);
        if(ammount < 0){ ammount = 0; }

        armour -= armourDamage;
        if(armour < 0){ armour = 0; }

        return remainingDamage;
    }

    void PlaySoundFX(AudioClip sound)
    {
        soundSource.pitch = Random.Range(0.9f, 1.1f);
        soundSource.PlayOneShot(sound);
    }
}
