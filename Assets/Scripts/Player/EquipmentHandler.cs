using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{

    public GameObject equipmentInventory;
    public GameObject[] equipment;

    [SerializeField] Animator pWeaponAnimations;
    
    [HideInInspector] public GameObject currentEquipment;
    [HideInInspector] public string currentEquipmentName;

    int currentIndex;


    public void SelectNextEquipment()
    {
        ReturnHandToIdle();

        currentIndex += 1;
        if(currentIndex >= equipment.Length){ currentIndex = 0; }

        if(currentEquipment != null){ currentEquipment.SetActive(false); }
        currentEquipment = equipment[currentIndex];
        currentEquipmentName = currentEquipment.GetComponent<AttackController>().weaponName;
        currentEquipment.SetActive(true);

        PickHandPosition();
    }

    public void SelectEquipment(int index)
    {
        ReturnHandToIdle();
        currentIndex = index;
        if(currentIndex > 0 && currentIndex < equipment.Length){
            Debug.LogError("Equipment currentIndex " + currentIndex + " out of range for equipment list of size " + equipment.Length + ". Clamping value");
            currentIndex = Mathf.Clamp(currentIndex, 0, equipment.Length-1);
        }

        if(currentEquipment != null){ currentEquipment.SetActive(false); }
        currentEquipment = equipment[currentIndex];
        currentEquipmentName = currentEquipment.GetComponent<AttackController>().weaponName;
        currentEquipment.SetActive(true);
        PickHandPosition();
    }

    // Start is called before the first frame update
    void Start()
    {
        //equipmentInventory.SetActive(false);
        currentIndex = 0;
    }

    void Awake()
    {
        SelectEquipment(currentIndex);
    }

    void ReturnHandToIdle(){
        switch (currentEquipmentName)
        {
            case "automatic":
                pWeaponAnimations.SetBool("isFingerRoll", false);
                break;
            case "scatterShot":
                pWeaponAnimations.SetBool("isFullHand", false);
                break;
            case "fireBall":
                pWeaponAnimations.SetBool("isFullHand", false);
                break;
            default:
                pWeaponAnimations.SetBool("noWeapon", true);
                Debug.LogWarning("Could not find animaton case for " + currentEquipmentName);
                break;
        }
    }

    void PickHandPosition(){
        switch (currentEquipmentName)
        {
            case "automatic":
                pWeaponAnimations.SetBool("isFingerRoll", true);
                break;
            case "scatterShot":
                pWeaponAnimations.SetBool("isFullHand", true);
                break;
            case "fireBall":
                pWeaponAnimations.SetBool("isFullHand", true);
                break;
            default:
                pWeaponAnimations.SetBool("noWeapon", true);
                Debug.LogWarning("Could not find animaton case for " + currentEquipmentName);
                break;
        }
    }
}
