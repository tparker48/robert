public class Item : JsonDataObject
{
    public int id { get; set; }
    public uint value { get; set; }
}

public static class Items
{
    private static JsonDataLookup<Item> lookup;

    public static JsonDataLookup<Item> GetInstance()
    {
        if (lookup == null)
        {
            lookup = new JsonDataLookup<Item>("Assets/Data/Items.json");
        }
        return lookup;
    }

    public static Item Lookup(string name)
    {
        return GetInstance()[name];
    }

    public static bool ItemExists(string itemName)
    {
        return GetInstance().GetTable().ContainsKey(itemName);
    }
}
