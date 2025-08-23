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

    public void PlantSeeds(InventoryItem seeds)
    {
        if (Empty())
        {
            plant = CreatePlantFromSeeds(seeds);
        }
    }

    public Dictionary<InventoryItem, uint> Harvest()
    {
        Dictionary<InventoryItem, uint> plantLoot = new Dictionary<InventoryItem, uint>();

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

    private Plant CreatePlantFromSeeds(InventoryItem seeds)
    {
        switch (seeds)
        {
            case InventoryItem.LettuceSeeds:
                return new Lettuce();
            default:
                return null;
        }
    }
}
