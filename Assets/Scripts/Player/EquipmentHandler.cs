using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentHandler : MonoBehaviour
{

    public GameObject[] equipment;
    public GameObject currentEquipment;

    [SerializeField] PlayerData pData;

    int currentIndex;


    public void SelectNextEquipment(Hand whichHand)
    {
        currentIndex += 1;
        if(currentIndex >= equipment.Length){ currentIndex = 0; }
        SelectEquipment(whichHand, currentIndex);
    }

    public void SelectEquipment(Hand whichHand, int index)
    {
        currentIndex = index;
        if(currentIndex < 0 || currentIndex >= equipment.Length){
            Debug.LogError("Equipment currentIndex " + currentIndex + " out of range for equipment list of size " + equipment.Length + ". Clamping value");
            currentIndex = Mathf.Clamp(currentIndex, 0, equipment.Length-1);
        }

        currentEquipment = equipment[currentIndex];
        if(whichHand == Hand.Left){ pData.leftItem  = currentEquipment; }
        else{ pData.rightItem = currentEquipment; }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
        SelectEquipment(Hand.Left, currentIndex);
        SelectEquipment(Hand.Right, currentIndex);
    }
}
