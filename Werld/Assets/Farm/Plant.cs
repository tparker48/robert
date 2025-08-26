using System.Collections.Generic;
using UnityEngine;

public enum PlantPhase
{
    seedling,
    young,
    mature,
    withered
}

public abstract class Plant
{
    public float ageInSeconds;
    public float[] phaseTimes;
    public PlantPhase phase;
    public string plantName;
    public Item seedItemEnum;

    public Plant()
    {
        ageInSeconds = 0.0f;
        phaseTimes = GetGrowthPhaseTimes();
        Debug.Log("Plant Created!");
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
            if (ageInSeconds >= phaseTimes[phaseNum-1])
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
            return phaseTimes[(int)PlantPhase.mature] - ageInSeconds;
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
            return new ItemGroup { { Item.DryLeaves, 4 } };
        }
        else
        {
            // should not reach this, but return empty dict just in case
            return new ItemGroup();
        }
    }

    protected abstract ItemGroup GetHarvestLoot(float harvestMultiplier);
    protected abstract float[] GetGrowthPhaseTimes();
}

public class Lettuce : Plant
{
    new public string plantName = "Lettuce";
    new public Item seedItemEnum = Item.LettuceSeeds;

    override protected ItemGroup GetHarvestLoot(float harvestMultiplier)
    {
        Debug.Log("Lettuce Harvested!");

        uint lettuceDrop = (uint)(Random.Range(6, 12) * harvestMultiplier);
        uint seedDrop = (uint)(Random.Range(0, 3) * harvestMultiplier);

        return new ItemGroup
        {
            {Item.Lettuce, lettuceDrop},
            {Item.LettuceSeeds, seedDrop}
        };
    }

    override protected float[] GetGrowthPhaseTimes()
    {
        return new float[3] {
            5.0f,   // young at 5s
            10.0f,  // mature at 10s
            25.0f   // withered at 25s
        };
    }
}
