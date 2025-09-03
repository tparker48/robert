using UnityEngine;

public class PlanterJob
{
    public PlantCommand plantCommand = null;
    public HarvestCommand harvestCommand = null;
}

public class RobertPlanter : RobertTimedTaskExecutor<PlanterJob>
{
    private ItemContainer inventory;
    private RobertSensors sensors;

    public void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensors>();
    }

    public void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    public void HandlePlantCommand(PlantCommand plantCommand)
    {
        PlanterJob job = new PlanterJob();
        job.plantCommand = plantCommand;
        busyText = "Planting";
        StartTimedTask(job, 5.0f);
    }

    public void HandleHarvestCommand(HarvestCommand harvestCommand)
    {
        PlanterJob job = new PlanterJob();
        job.harvestCommand = harvestCommand;
        busyText = "Harvesting";
        StartTimedTask(job, 1.0f);
    }

    public void ProcessPlantCommand(PlantCommand plantCommand)
    {
        Item seedItem = Items.Lookup(plantCommand.seed_item);
        GrowBox box = null;
        if (sensors.GetObjectOfType(ref box))
        {
            if (box.Empty() && inventory.GetItemCount(seedItem) > 0)
            {
                if (box.PlantSeeds(seedItem))
                {
                    inventory.RemoveItem(seedItem, 1);
                }
            }
        }
    }

    public void ProcessHarvestCommand(HarvestCommand _)
    {
        GrowBox box = null;
        if (sensors.GetObjectOfType(ref box))
        {
            if (box.CanHarvest())
            {
                box.Harvest(ref inventory);
            }
        }
    }

    protected override void ExecuteOnTaskEnd(PlanterJob job)
    {
        if (job.plantCommand != null)
        {
            ProcessPlantCommand(job.plantCommand);
        }
        else if (job.harvestCommand != null)
        {
            ProcessHarvestCommand(job.harvestCommand);
        }

    }

    public Response HandleCheckGrowBoxStatus(CheckGrowBoxStatus _)
    {
        CheckGrowBoxStatusResponse response = new CheckGrowBoxStatusResponse();
        response.grow_box_in_range = false;

        GrowBox box = null;
        if (sensors.GetObjectOfType(ref box))
        {
            response.grow_box_in_range = true;
            PlantEntity plantEntity = box.GetPlant();
            if (plantEntity != null)
            {
                response.has_plant = true;
                response.plant_name = plantEntity.plant.name;
                response.ready_to_harvest = box.CanHarvest();
                response.plant_age_seconds = plantEntity.ageInSeconds;
                response.plant_phase_times = plantEntity.plant.growthPhaseTimes;
            }
        }
        return response;
    }
}
