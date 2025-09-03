using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;

public class GrowBox : MonoBehaviour
{
    private PlantEntity plant;
    private float harvestMultiplier = 1.0f;

    public Renderer seedlingModel;
    public Renderer youngModel;
    public Renderer matureModel;
    public Renderer witheredModel;

    void Start()
    {

    }

    void Update()
    {
        seedlingModel.enabled = false;
        youngModel.enabled = false;
        matureModel.enabled = false;
        witheredModel.enabled = false;

        if (plant != null)
        {
            plant.Update();

            switch (plant.phase)
            {
                case PlantPhase.seedling:
                    seedlingModel.enabled = true;
                    break;
                case PlantPhase.young:
                    youngModel.enabled = true;
                    break;
                case PlantPhase.mature:
                    matureModel.enabled = true;
                    break;
                case PlantPhase.withered:
                    witheredModel.enabled = true;
                    break;
            }
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

    public void Harvest(ref ItemContainer other)
    {
        if (CanHarvest())
        {
            ItemGroup plantLoot = plant.Harvest(harvestMultiplier);
            if (other.AddItems(plantLoot))
            {
                plant = null;
            }
        }
    }

    public bool CanHarvest()
    {
        return plant != null && (uint)plant.phase >= (uint)PlantPhase.mature;
    }

    public PlantEntity GetPlant()
    {
        return plant;
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
