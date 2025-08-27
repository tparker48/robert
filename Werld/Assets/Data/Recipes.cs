
using System.Collections.Generic;

public class Recipe : JsonDataObject
{
    public uint level { get; set; }
    public float printTime { get; set; }
    public Dictionary<string, uint> inputs { get; set; }
}

public static class Recipes
{
    private static JsonDataLookup<Recipe> lookup;

    public static JsonDataLookup<Recipe> GetInstance()
    {
        if (lookup == null)
        {
            lookup = new JsonDataLookup<Recipe>("Assets/Data/Recipes.json");
        }
        return lookup;
    }

    public static Recipe Lookup(string name)
    {
        return GetInstance()[name];
    }

    public static bool RecipeExists(string name)
    {
        return GetInstance().GetTable().ContainsKey(name);
    }
}
