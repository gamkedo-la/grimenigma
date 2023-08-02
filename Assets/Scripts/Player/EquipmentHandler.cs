using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{

    public GameObject equipmentInventory;
    public GameObject[] equipment;

    [HideInInspector] public GameObject currentEquipment;

    int currentIndex;


    public void SelectNextEquipment()
    {
        currentIndex += 1;
        if(currentIndex >= equipment.Length){ currentIndex = 0; }

        if(currentEquipment != null){ currentEquipment.SetActive(false); }
        currentEquipment = equipment[currentIndex];
        currentEquipment.SetActive(true);
    }

    public void SelectEquipment(int index)
    {
        currentIndex = index;
        if(currentIndex > 0 && currentIndex < equipment.Length){
            Debug.LogError("Equipment currentIndex " + currentIndex + " out of range for equipment list of size " + equipment.Length + ". Clamping value");
            currentIndex = Mathf.Clamp(currentIndex, 0, equipment.Length-1);
        }

        if(currentEquipment != null){ currentEquipment.SetActive(false); }
        currentEquipment = equipment[currentIndex];
        currentEquipment.SetActive(true);
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

}