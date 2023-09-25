using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectHandAnimations : MonoBehaviour
{
    [SerializeField] Animator left;
    [SerializeField] Animator right;

    Animator anim;

    public void PlayAttackAnim(Hand whichHand)
    {
        anim = SetHand(whichHand);
        anim.SetTrigger("attack");
    }

    public void PickHandPosition(Hand whichHand, ItemID item, bool isActive){
        anim = SetHand(whichHand);

        switch (item)
        {
            case ItemID.Automatic:
                anim.SetBool("noWeapon", isActive);
                break;
            case ItemID.ScatterShot:
                anim.SetBool("isFingerRoll", isActive);
                break;
            case ItemID.Fireball:
                anim.SetBool("isFullHand", isActive);
                break;
            case ItemID.Charge:
                anim.SetBool("isFingerGun", isActive);
                break;
            default:
                anim.SetBool("noWeapon", true);
                Debug.LogError("Could not find animaton case for " + name);
                break;
        }
    }

void Start()
    {
        PickHandPosition(Hand.Left, ItemID.Automatic, true);
        PickHandPosition(Hand.Right, ItemID.Automatic, true);
    }

    private Animator SetHand(Hand whichHand)
    {
        Animator anim;

        switch (whichHand)
        {
            case Hand.Left:
                anim = left;
                break;
            case Hand.Right:
                anim = right;
                break;
            default:
                Debug.LogError("Could not find case for Hand of type " + whichHand + "!");
                // This looks dirty, but I am going with it.
                anim = null;
                break;
        }

        return anim;
    }
}
