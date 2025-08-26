using System.Collections.Generic;
using UnityEngine;

public class GrowBox : MonoBehaviour
{
    private Plant plant;
    private float harvestMultiplier = 1.0f;

    void Start()
    {

    }

    void Update()
    {
        if (plant != null)
        {
            plant.Update();
        }
    }

    public bool PlantSeeds(Item seeds)
    {
        if (Empty())
        {
            plant = CreatePlantFromSeeds(seeds);
        }
        return plant != null;
    }

    public ItemGroup Harvest()
    {
        ItemGroup plantLoot = new ItemGroup();

        if (CanHarvest())
        {
            plantLoot = plant.Harvest(harvestMultiplier);
            plant = null;
            return plantLoot;
        }
        else
        {
            return plantLoot;
        }
    }

    public bool CanHarvest()
    {
        return plant != null && (uint)plant.phase >= (uint)PlantPhase.mature;
    }

    public bool Empty()
    {
        return plant == null;
    }

    private Plant CreatePlantFromSeeds(Item seeds)
    {
        switch (seeds)
        {
            case Item.LettuceSeeds:
                return new Lettuce();
            default:
                return null;
        }
    }
}
