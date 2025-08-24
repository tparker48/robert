using System.Collections.Generic;
using UnityEngine;

public class RobertPlanter : RobertTimedTaskExecutor<PlantCommand>
{
    private RobertInventory inventory;
    private RobertSensors sensors;

    public void Start()
    {
        inventory = GetComponentInParent<RobertInventory>();
        sensors = GetComponentInParent<RobertSensors>();
    }

    public void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    public void HandlePlantCommand(PlantCommand plantCommand)
    {
        StartTimedTask(plantCommand, 5.0f);
    }

    public void ProcessPlantCommand(PlantCommand plantCommand)
    {
        InventoryItem seedEnum = (InventoryItem)plantCommand.seed_item_id;
        if (sensors.CheckForObjectType<GrowBox>())
        {
            Debug.Log("Found Grow Box for PlantCommand!");
            GrowBox box = sensors.GetObjectType<GrowBox>();
            if (box.Empty() && inventory.GetItemCount(seedEnum) > 0)
            {
                Debug.Log("GrowBox is empty and we have at least 1 seed!");
                if (box.PlantSeeds(seedEnum))
                {
                    Debug.Log("Planted!");
                    inventory.RemoveItem(seedEnum, 1);
                }
            }
        }
    }

    protected override void ExecuteOnTaskEnd(PlantCommand plantCommand)
    {
        ProcessPlantCommand(plantCommand);
    }
}
