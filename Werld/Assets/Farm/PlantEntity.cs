using UnityEngine;


public enum PlantPhase
{
    seedling,
    young,
    mature,
    withered
}

public class PlantEntity
{
    public Plant plant;
    private float ageInSeconds;
    public PlantPhase phase;

    public PlantEntity(Plant plant)
    {
        ageInSeconds = 0.0f;
        this.plant = plant;
    }

    public void Update()
    {
        ageInSeconds += Time.deltaTime;
        UpdatePlantPhase();
    }

    private void UpdatePlantPhase()
    {
        for (uint phaseNum = (uint)PlantPhase.withered; phaseNum >= (uint)PlantPhase.young; phaseNum--)
        {
            if (ageInSeconds >= plant.growthPhaseTimes[phaseNum - 1])
            {
                phase = (PlantPhase)phaseNum;
                return;
            }
        }
    }

    public float GetRemainingTimeToMaturity()
    {
        if (phase == PlantPhase.mature || phase == PlantPhase.withered)
        {
            return 0.0f;
        }
        else
        {
            return plant.growthPhaseTimes[(int)PlantPhase.mature] - ageInSeconds;
        }
    }

    public ItemGroup Harvest(float harvestMultiplier)
    {
        if (phase == PlantPhase.mature)
        {
            return GetHarvestLoot(harvestMultiplier);
        }
        else if (phase == PlantPhase.withered)
        {
            Debug.Log("Dead Plant Harvested!");
            return new ItemGroup { { Items.Lookup("Dry Leaves"), 4 } };
        }
        else
        {
            // should not reach this, but return empty dict just in case
            return new ItemGroup();
        }
    }

    public ItemGroup GetHarvestLoot(float harvestMultiplier)
    {
        Debug.Log("Lettuce Harvested!");

        uint lettuceDrop = (uint)(harvestMultiplier * Random.Range(
            (uint)plant.harvestDropRange[0],
            (uint)plant.harvestDropRange[1] 
            ));
        uint seedDrop = (uint)Random.Range(
            (uint)plant.seedDropRange[0],
            (uint)plant.seedDropRange[1]
            );
        return new ItemGroup
        {
            { Items.Lookup(plant.harvestItem), lettuceDrop},
            { Items.Lookup(plant.seedItem), seedDrop}
        };
    }
}