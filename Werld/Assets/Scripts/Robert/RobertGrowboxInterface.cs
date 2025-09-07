using UnityEngine;

public class PlanterJob
{
    public PlantSeed plantCommand = null;
    public Harvest harvestCommand = null;
}

public class RobertGrowboxInterface : RobertTimedTaskExecutor<PlanterJob>
{
    private ItemContainer inventory;
    private RobertSensorInterface sensors;

    public void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensorInterface>();
    }

    public void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    public void HandlePlantSeed(string rawCmd)
    {
        PlantSeed plantCommand = CommandParser.Parse<PlantSeed>(rawCmd);
        PlanterJob job = new PlanterJob();
        job.plantCommand = plantCommand;
        busyText = "Planting";
        StartTimedTask(job, 5.0f);
    }

    public void HandleHarvest(string rawCmd)
    {
        Harvest harvestCommand = CommandParser.Parse<Harvest>(rawCmd);
        PlanterJob job = new PlanterJob();
        job.harvestCommand = harvestCommand;
        busyText = "Harvesting";
        StartTimedTask(job, 1.0f);
    }

    public void ProcessPlantSeed(PlantSeed plantCommand)
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

    public void ProcessHarvest(Harvest _)
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
            ProcessPlantSeed(job.plantCommand);
        }
        else if (job.harvestCommand != null)
        {
            ProcessHarvest(job.harvestCommand);
        }

    }

    public Response HandleCheckGrowBoxStatus(string _)
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
