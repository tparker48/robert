using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemContainer : MonoBehaviour
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

    public bool AddItems(Dictionary<Item, uint> loot)
    {
        Dictionary<Item, uint> newState = new Dictionary<Item, uint>(items);
        foreach (Item item in loot.Keys)
        {
            if (newState.ContainsKey(item))
            {
                newState[item] += loot[item];
            }
            else
            {
                newState[item] = loot[item];
            }
            if (newState.Count > capacity || newState[item] > maxItemStackSize)
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

    public bool RemoveItems(Dictionary<Item, uint> remove)
    {
        Dictionary<Item, uint> newState = new Dictionary<Item, uint>(items);
        foreach (Item item in remove.Keys)
        {
            if (newState.ContainsKey(item) && newState[item] >= remove[item])
            {
                newState[item] -= remove[item];
            }
            else
            {
                return false;
            }
        }

        items = newState;
        return true;
    }

    public Dictionary<Item, uint> GetInventory()
    {
        return items;
    }

    public List<Item> GetItemsList()
    {
        return items.Keys.ToList();
    }

    public uint GetItemCount(Item item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }

    public bool HasRoomFor(Item item, uint quantity)
    {
        if (items.ContainsKey(item))
        {
            return items[item] + 1 <= maxItemStackSize;
        }
        else
        {
            return items.Count + quantity <= capacity;
        }
    }

    public void Clear()
    {
        items.Clear();
    }
}
