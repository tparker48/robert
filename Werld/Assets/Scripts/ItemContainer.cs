using System.Collections.Generic;
using UnityEngine;

public class ItemContainer: MonoBehaviour
{
    private Dictionary<Item, uint> items = new Dictionary<Item, uint>();
    private uint capacity = 4;
    private uint maxItemStackSize = 64;

    public void SetCapacity(uint capacity)
    {
        this.capacity = capacity;
    }

    public bool AddItem(Item item, uint quantity)
    {
        if (items.ContainsKey(item))
        {
            if (items[item] + quantity <= maxItemStackSize)
            {
                items[item] += quantity;
                return true;
            }
        }
        else
        {
            if (items.Keys.Count + 1 <= capacity)
            {
                items.Add(item, quantity);
                return true;
            }
        }
        return false;
    }

    public bool AddFrom(Dictionary<Item, uint> loot)
    {
        Dictionary<Item, uint> newState = new Dictionary<Item, uint>(items);
        foreach (Item item in loot.Keys)
        {
            if (!AddItem(item, loot[item]))
            {
                return false;
            }
        }
        items = newState;
        return true;
    }

    public bool RemoveItem(Item item, uint quantity)
    {
        if (!items.ContainsKey(item)) return false;
        if (items[item] < quantity) return false;
        items[item] -= quantity;
        return true;
    }

    public Dictionary<Item, uint> GetInventory()
    {
        return items;
    }

    public uint GetItemCount(Item item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }
}
