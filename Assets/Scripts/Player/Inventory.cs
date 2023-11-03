using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Options")]
    [SerializeField] bool infiniteInventory = true;
    [SerializeField] int size;

    private Dictionary<ItemID, int> inventory;

    public int AddItem(ItemID item, int quantity)
    {
        /*
        Key:
            1   :   Success.
            -1  :   Inventory full.
            -2  :   Trying to add item with ammount < 0.
        */

        if(!infiniteInventory && inventory.Count >= size){ return -1; }

        if(quantity < 1){
            Debug.LogError("Trying to add item " + item + " to inventory with quantity of " + quantity + "! Quantity must be greater than 1. If reducing ammount of item in inventory, try using UseItem(" + quantity + ") instead.");
            return -2;
        }

        if(!inventory.ContainsKey(item)){ inventory.Add(item, 0); }
        inventory[item] += quantity;

        //Debug.Log("Gained inventory item!");

        return 1;
    }

    public bool HasItem(ItemID item)
    {
        return inventory.ContainsKey(item) && inventory[item] > 0;
    }

    public int UseItem(ItemID item, int quantity)
    {
        /*
        Key:
            1   :   Success.
            -1  :   Inventory does not contain item.
            -2  :   Quantity exceeds ammount inventory contains.
        */

        if(!HasItem(item)){ return -1; }
        if(inventory[item] < quantity){ return -2; }

        inventory[item] -= quantity;
        if(inventory[item] < 0){ inventory.Remove(item); }
        return 1;
    }

    void Start()
    {
        inventory = new Dictionary<ItemID, int>();
    }

}
