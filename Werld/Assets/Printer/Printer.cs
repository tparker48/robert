using System;
using System.Collections.Generic;
using UnityEngine;

public class PrintJob
{
    public Item printItem;
    public float printTime;
}

public class Printer : RobertTimedTaskExecutor<PrintJob>
{
    private uint printerLevel = 1;
    private float printSpeed = 1.0f;
    private ItemContainer inputs;
    private ItemContainer outputs;
    private Queue<PrintJob> printerQueue = new Queue<PrintJob>();

    public void Start()
    {
        inputs = GetComponents<ItemContainer>()[0];
        outputs = GetComponents<ItemContainer>()[1];

        // for debug
        inputs.AddItem(Items.Lookup("Stone"), 64);
        outputs.AddItem(Items.Lookup("Copper Ingot"), 2);
    }

    public void Update()
    {
        UpdateTask(Time.deltaTime);
        if (!IsBusy() && printerQueue.Count != 0)
        {
            PrintJob nextJob = printerQueue.Dequeue();
            if (CanPrint(nextJob))
            {
                StartTimedTask(nextJob, nextJob.printTime);
            }
        }
    }

    public bool AddInputsFrom(ref ItemContainer source, ItemGroup itemsToAdd)
    {
        return inputs.TakeItemsFrom(ref source, itemsToAdd);
    }

    public bool RemoveInputsTo(bool collectAll, ref ItemContainer other, ItemGroup itemsToRemove)
    {
        if (collectAll)
        {
            itemsToRemove = new ItemGroup(inputs.GetInventory());
        }
        return other.TakeItemsFrom(ref inputs, itemsToRemove);
    }

    public bool RemoveOutputsTo(bool collectAll, ref ItemContainer other, ItemGroup itemsToRemove)
    {
        if (collectAll)
        {
            itemsToRemove = new ItemGroup(outputs.GetInventory());
        }
        return other.TakeItemsFrom(ref outputs, itemsToRemove);
    }

    public bool QueuePrintJob(Item printItem, int quantity)
    {
        if (!outputs.HasRoomFor(printItem, (uint)quantity))
        {
            return false;
        }

        if (Recipes.RecipeExists(printItem.name))
        {
            Recipe recipe = Recipes.Lookup(printItem.name);
            for (int i = 0; i < quantity; i++)
            {
                PrintJob printJob = new PrintJob();
                printJob.printItem = printItem;
                printJob.printTime = recipe.printTime * printSpeed;
                printerQueue.Enqueue(printJob);
            }
            return true;
        }
        return false;
    }

    private void ExecutePrintJob(PrintJob printJob)
    {
        Item printItem = printJob.printItem;
        Recipe recipe = Recipes.Lookup(printItem.name);
        foreach (Dictionary<string, uint> inputsDict in recipe.inputs)
        {
            ItemGroup recipeInputs = new ItemGroup(inputsDict);

            // check we have enough of the required inputs
            foreach (Item item in recipeInputs.Keys)
            {
                if (inputs.GetItemCount(item) < recipeInputs[item])
                {
                    return;
                }
            }

            // remove required inputs
            foreach (Item item in recipeInputs.Keys)
            {
                inputs.RemoveItem(item, recipeInputs[item]);
            }

            // add output
            outputs.AddItem(printItem, 1);
            return;
        }
    }

    protected override void ExecuteOnTaskEnd(PrintJob task)
    {
        ExecutePrintJob(task);
    }

    public PrinterStatusQueryResponse HandlePrinterStatusQuery(PrinterStatusQuery _)
    {
        PrinterStatusQueryResponse resp = new PrinterStatusQueryResponse();
        resp.busy = IsBusy();
        resp.inputs = new Dictionary<string, uint>();
        resp.outputs = new Dictionary<string, uint>();
        foreach (Item input in inputs.GetItemsList())
        {
            resp.inputs[input.name] = inputs.GetItemCount(input);
        }
        foreach (Item output in outputs.GetItemsList())
        {
            resp.outputs[output.name] = outputs.GetItemCount(output);
        }
        return resp;
    }

    public void Stop()
    {
        Halt();
        printerQueue.Clear();
    }

    private bool CanPrint(PrintJob printJob)
    {
        Item printItem = printJob.printItem;
        Recipe recipe = Recipes.Lookup(printItem.name);
        if (recipe == null) return false;

        foreach (Dictionary<string, uint> inputsDict in recipe.inputs)
        {
            ItemGroup recipeInputs = new ItemGroup(inputsDict);

            // check we have enough of the required inputs
            foreach (Item item in recipeInputs.Keys)
            {
                if (inputs.GetItemCount(item) < recipeInputs[item])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
