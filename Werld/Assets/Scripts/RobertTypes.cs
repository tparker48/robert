using System.Collections.Generic;


public class RobertType : JsonDataObject 
{
    public uint initialCost { get; set; }
    public float moveSpeed { get; set; }
    public uint capacity { get; set; }
    public float mineSpeed { get; set; }
    public float harvestMultiplier { get; set; }
    public float sellValueMultiplier { get; set; }
    public string upgradeTrait { get; set; }
    public float[] traitValues { get; set; }
    public List<Dictionary<string, uint>> upgradeMats { get; set; }
}

public class RobertTypes
{
    private static JsonDataLookup<RobertType> lookup;

    public static JsonDataLookup<RobertType> GetInstance()
    {
        if (lookup == null)
        {
            lookup = new JsonDataLookup<RobertType>("Assets/Data/RobertTypes.json");
        }
        return lookup;
    }

    public static RobertType Lookup(string name)
    {
        return GetInstance()[name];
    }

    public static bool RobertTypeExists(string name)
    {
        return GetInstance().GetTable().ContainsKey(name);
    }
}