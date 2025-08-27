using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class GrowBox : MonoBehaviour
{
    private PlantEntity plant;
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

    private PlantEntity CreatePlantFromSeeds(Item seeds)
    {
        Plant plantToGrow = Plants.FromSeeds(seeds);
        if (seeds != null)
        {
            return new PlantEntity(plantToGrow);
        }
        else
        {
            return null;
        }
    }
}
