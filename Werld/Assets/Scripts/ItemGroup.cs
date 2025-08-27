using System;
using System.Collections.Generic;

// It's just a Dictionary<Item, uint> with some helpers to converting to/from Dictionary<string, uint>

public class ItemGroup : Dictionary<Item, uint>
{
    public ItemGroup() : base() { }

    public ItemGroup(ItemGroup other)
    {
        foreach (Item item in other.Keys)
        {
            Add(item, other[item]);
        }
    }

    public ItemGroup(Dictionary<string, uint> stringKeys)
    {
        foreach (string itemName in stringKeys.Keys)
        {
            Item item = Items.Lookup(itemName);
            Add(item, stringKeys[itemName]);
        }
    }

    public Dictionary<string, uint> ToStringKeys()
    {
        Dictionary<string, uint> stringKeys = new Dictionary<string, uint>();
        foreach (Item item in this.Keys)
        {
            stringKeys[item.name] = this[item];
        }
        return stringKeys;
    }
}
