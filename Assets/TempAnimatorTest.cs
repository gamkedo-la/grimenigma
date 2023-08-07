using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAnimatorTest : MonoBehaviour
{
    public Animator anim;
    private void Update()
    {
        // These key presses act as a 'Weapon Switch'
        if (Input.GetKey(KeyCode.U))
        {
            TurnOffOtherBools();
            anim.SetBool("isFingerGun", true);
        }
        if (Input.GetKey(KeyCode.I))
        {
            TurnOffOtherBools();
            anim.SetBool("isFingerRoll", true);
        }
        if (Input.GetKey(KeyCode.O))
        {
            TurnOffOtherBools();
            anim.SetBool("isFist", true);
        }
        if (Input.GetKey(KeyCode.P))
        {
            TurnOffOtherBools();
            anim.SetBool("isFullHand", true);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            TurnOffOtherBools();
            anim.SetBool("noWeapon", true);
        }

        //This indicates which attack to do based on which weapon is pulled
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(anim.GetBool("isFingerGun"))
                anim.SetTrigger("fingerGunAttack");
            if (anim.GetBool("isFingerRoll"))
                anim.SetTrigger("fingerRollAttack");
            if (anim.GetBool("isFist"))
                anim.SetTrigger("fistAttack");
            if (anim.GetBool("isFullHand"))
                anim.SetTrigger("fullHandAttack");
        }
    }

    void TurnOffOtherBools()
    {
        //Just a helper function to not have any loose ends when switching between weapons
        anim.SetBool("noWeapon", false);
        anim.SetBool("isFingerGun", false);
        anim.SetBool("isFingerRoll", false);
        anim.SetBool("isFist", false);
        anim.SetBool("isFullHand", false);
    }
}
