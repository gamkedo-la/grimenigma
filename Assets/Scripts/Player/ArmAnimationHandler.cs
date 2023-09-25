using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class ArmAnimationHandler : MonoBehaviour
{
    [SerializeField] EquipmentHandler myEquipment;

    [SerializeField] Animator animator;

    GameObject currentEquipment;

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    public void PickHandPosition(ItemID item, bool isActive){
        switch (item)
        {
            case ItemID.Automatic:
                animator.SetBool("noWeapon", isActive);
                break;
            case ItemID.ScatterShot:
                animator.SetBool("isFingerRoll", isActive);
                break;
            case ItemID.Fireball:
                animator.SetBool("isFullHand", isActive);
                break;
            case ItemID.Charge:
                animator.SetBool("isFingerGun", isActive);
                break;
            default:
                animator.SetBool("noWeapon", true);
                Debug.LogError("Could not find animaton case for " + name);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {


        currentEquipment = myEquipment.currentEquipment;
        PickHandPosition(currentEquipment.GetComponent<ItemIDController>().id, true);
    }

    void OnEnable()
    {
        AttackController ac;
        foreach(GameObject equipment in myEquipment.equipment){
            ac = equipment.GetComponent<AttackController>();
            ac.onAttack += PlayAttackAnimation;
            ac.onCharging += PlayChargeAnimation;
        }
    }

    void OnDisable()
    {
        AttackController ac;
        foreach(GameObject equipment in myEquipment.equipment){
            ac = equipment.GetComponent<AttackController>();
            ac.onAttack -= PlayAttackAnimation;
            ac.onCharging -= PlayChargeAnimation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentEquipment!= myEquipment.currentEquipment){
            PickHandPosition(currentEquipment.GetComponent<ItemIDController>().id, false);
            currentEquipment = myEquipment.currentEquipment;
            PickHandPosition(currentEquipment.GetComponent<ItemIDController>().id, true);
            ResetTriggers();
        }
    }

    void PlayChargeAnimation()
    {
        animator.SetTrigger("charge");
    }

    void ResetTriggers()
    {
        animator.ResetTrigger("attack");
        animator.ResetTrigger("charge");
    }
}
