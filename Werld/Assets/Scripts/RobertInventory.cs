using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertInventory : MonoBehaviour
{
    private Dictionary<InventoryItem, uint> inventory;

    // Start is called before the first frame update
    void Start()
    {
        inventory = new Dictionary<InventoryItem, uint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddItem(InventoryItem item, uint quantity)
    {
        if (!inventory.ContainsKey(item))
        {
            inventory.Add(item, 0);
        }

        inventory[item] += quantity;
    }

    public void AddFrom(Dictionary<InventoryItem, uint> loot)
    {
        foreach(InventoryItem item in loot.Keys){
            AddItem(item, loot[item]);
        }
    }

    public bool RemoveItem(InventoryItem item, uint quantity)
    {
        if (!inventory.ContainsKey(item)) return false;
        if (inventory[item] < quantity) return false;
        inventory[item] -= quantity;
        return true;
    }

    public Dictionary<InventoryItem, uint> GetInventory()
    {
        return inventory;
    }

    public int GetItemCount(int itemId)
    {
        if (IsValidItemId(itemId))
        {
            InventoryItem item = (InventoryItem)itemId;
            return (int) (inventory.ContainsKey(item) ? inventory[item] : 0);
        }
        return -1;
    }

    public static bool IsValidItemId(int itemId)
    {
        return itemId >= 0 && itemId < (uint)InventoryItem.None;
    }

    public uint GetItemCount(InventoryItem item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }
}
