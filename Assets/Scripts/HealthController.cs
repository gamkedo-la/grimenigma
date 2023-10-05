using System.Collections.Generic;
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

    public event System.Action<int, GameObject> onDamage;

    DeathController deathController;

   private void Start()
   {
        deathController = GetComponent<DeathController>();

        if(hasAudioFX && !TryGetComponent<AudioSource>(out soundSource)){
            soundSource = gameObject.AddComponent<AudioSource>();
        }

        hp = baseHP;
        //Debug.Log("Start HP: " + hp);
   }
    AudioSource soundSource;

    public void Damage(int ammount, GameObject damageSource, bool piercingDamage=false)
    {
        onDamage?.Invoke(ammount, damageSource);
        if(hasAudioFX){ PlaySoundFX(onDamageSound); }
        if(!godMode){
            if(!piercingDamage){ ammount = ArmourReduction(ammount); }
            hp -= ammount;
            //Debug.Log("Recieved " + ammount + " damage!");

            if(hp < 1){
                //Debug.Log("I am dead!");
                deathController.HandleDeath();
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
