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
    private ItemContainer inputs = new ItemContainer();
    private ItemContainer outputs = new ItemContainer();
    private Queue<PrintJob> printerQueue = new Queue<PrintJob>();

    public void Update()
    {
        UpdateTask(Time.deltaTime);
        if (!IsBusy() && printerQueue.Count != 0)
        {
            PrintJob nextJob = printerQueue.Dequeue();
            StartTimedTask(nextJob, nextJob.printTime);
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
            itemsToRemove = new ItemGroup(other.GetInventory());
        }
        return other.TakeItemsFrom(ref inputs, itemsToRemove);
    }

    public bool RemoveOutputsTo(bool collectAll, ref ItemContainer other, ItemGroup itemsToRemove)
    {
        if (collectAll)
        {
            itemsToRemove = new ItemGroup(other.GetInventory());
        }
        return other.TakeItemsFrom(ref outputs, itemsToRemove);
    }

    public bool QueuePrintJob(Item printItem)
    {
        if (!outputs.HasRoomFor(printItem, 1))
        {
            return false;
        }

        if (PrinterRecipes.RecipeExists(printItem))
        {
            Recipe recipe = PrinterRecipes.GetRecipe(printItem);
            PrintJob printJob = new PrintJob();
            printJob.printItem = printItem;
            printJob.printTime = recipe.printTime * printSpeed;
            printerQueue.Enqueue(printJob);
            return true;
        }
        return false;
    }

    private void ExecutePrintJob(PrintJob printJob)
    {
        Item printItem = printJob.printItem;
        Recipe recipe = PrinterRecipes.GetRecipe(printItem);
        ItemGroup recipeInputs = new ItemGroup(recipe.inputs);
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
            resp.inputs[input.ToString()] = inputs.GetItemCount(input);
        }
        foreach (Item output in outputs.GetItemsList())
        {
            resp.outputs[output.ToString()] = outputs.GetItemCount(output);
        }
        return resp;
    }

    public void Stop()
    {
        Halt();
        printerQueue.Clear();
    }
}
