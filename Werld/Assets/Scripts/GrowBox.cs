using System.Collections.Generic;
using UnityEngine;

public class GrowBox : MonoBehaviour
{
    public List<PlantModel> plantModels = new List<PlantModel>();

    private PlantEntity plant;
    private PlantModel plantModel;
    private float harvestMultiplier = 1.0f;

    void Update()
    {
        if (plant != null)
        {
            plant.Update();
            plantModel.SetPhase(plant.phase);
        }
    }

    public bool PlantSeeds(Item seeds)
    {
        if (Empty())
        {
            CreatePlantFromSeeds(seeds);
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
                Destroy(plantModel);
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

    private void CreatePlantFromSeeds(Item seeds)
    {
        Plant plantToGrow = Plants.FromSeeds(seeds);
        Debug.Log(plantToGrow.name);
        if (seeds != null)
        {
            foreach (PlantModel model in plantModels)
            {
                if (model.plantName == plantToGrow.name)
                {
                    plant = new PlantEntity(plantToGrow);
                    plantModel = Instantiate(model, transform);
                }
            }
        }
    }


}
