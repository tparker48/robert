public class Plant: JsonDataObject
{
    public string harvestItem { get; set; }
    public string seedItem { get; set; }
    public float[] growthPhaseTimes { get; set; }
    public float[] harvestDropRange { get; set; }
    public float[] seedDropRange { get; set; }
}

public static class Plants
{
    private static JsonDataLookup<Plant> lookup;

    public static JsonDataLookup<Plant> GetInstance()
    {
        if (lookup == null)
        {
            lookup = new JsonDataLookup<Plant>("Assets/Data/Plants.json");
        }
        return lookup;
    }

    public static Plant Lookup(string name)
    {
        return GetInstance()[name];
    }

    public static Plant FromSeeds(Item seeds)
    {
        foreach (Plant plant in GetInstance().GetTable().Values)
        {
            if (plant.seedItem == seeds.name)
            {
                return plant;
            }
        }
        return null;
    }
}
