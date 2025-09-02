using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemContainer : MonoBehaviour
{
    private ItemGroup items = new ItemGroup();
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
            if (HasRoomFor(item, quantity))
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

    public bool AddItems(ItemGroup loot)
    {
        ItemGroup newState = new ItemGroup(items);
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

    public bool RemoveItems(ItemGroup remove)
    {
        ItemGroup newState = new ItemGroup(items);
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

    public ItemGroup GetInventory()
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
            return items[item] + quantity <= maxItemStackSize;
        }
        else
        {
            return items.Count + 1 <= capacity;
        }
    }

    public void Clear()
    {
        items.Clear();
    }

    public bool TakeItemsFrom(ref ItemContainer source, ItemGroup itemsToMove)
    {
        if (source.RemoveItems(itemsToMove))
        {
            if (AddItems(itemsToMove))
            {
                return true;
            }
            else
            {
                source.AddItems(itemsToMove);
            }
        }
        return false;
    }
}
