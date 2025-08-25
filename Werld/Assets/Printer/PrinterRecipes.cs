
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Recipes
{
    public Dictionary<string, Recipe> recipes {get; set; }
}

public class Recipe
{
    public uint level { get; set; }
    public uint quantity { get; set; }
    public Dictionary<string, uint> inputs { get; set; }
}

public static class PrinterRecipes
{
    public static Dictionary<string, Recipe> recipes = LoadRecipesFromJson();

    public static bool RecipeExists(Item item)
    {
        return recipes.ContainsKey(item.ToString());
    }

    public static Recipe GetRecipe(Item item)
    {
        if (RecipeExists(item))
        {
            return recipes[item.ToString()];
        }
        else
        {
            return null;
        }
    }

    private static Dictionary<string, Recipe> LoadRecipesFromJson()
    {
        string recipeJsonString = File.ReadAllText("Assets/Printer/recipes.json");
        Recipes recipeObject = JsonConvert.DeserializeObject<Recipes>(recipeJsonString);
        return recipeObject.recipes;
    }
}
