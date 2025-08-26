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

    public bool debugAddBitToInputs = false;
    public bool debugTryPrint = false;
    public bool debugPrintInput = false;
    public bool debugPrintOutput = false;
    public bool debugClearOutput = false;
    public void Update()
    {
        UpdateTask(Time.deltaTime);
        if (!IsBusy() && printerQueue.Count != 0)
        {
            PrintJob nextJob = printerQueue.Dequeue();
            StartTimedTask(nextJob, nextJob.printTime);
        }

        if (debugAddBitToInputs)
        {
            debugAddBitToInputs = false;
            AddInput(Item.Bits, 1);
        }

        if (debugTryPrint)
        {
            debugTryPrint = false;
            QueuePrintJob(Item.Bits);
        }

        if (debugPrintInput)
        {
            debugPrintInput = false;
            foreach (Item item in inputs.GetItemsList())
            {
                Debug.Log(item.ToString() + " " + inputs.GetItemCount(item));
            }
        }

        if (debugPrintOutput)
        {
            debugPrintOutput = false;
            foreach (Item item in outputs.GetItemsList())
            {
                Debug.Log(item.ToString() + " " + outputs.GetItemCount(item));
            }
        }

        if (debugClearOutput)
        {
            debugClearOutput = false;
            CollectOutputs(true, new Dictionary<Item, uint>());
        }
    }

    public bool AddInput(Item item, uint amount)
    {
        return inputs.AddItem(item, amount);
    }

    public Dictionary<Item, uint> CollectInputs(bool collectAll, Dictionary<Item, uint> itemsToCollect)
    {
        if (collectAll)
        {
            Dictionary<Item, uint> returnItems = new Dictionary<Item, uint>(inputs.GetInventory());
            inputs.Clear();
            return returnItems;
        }
        if (inputs.RemoveItems(itemsToCollect))
        {
            return itemsToCollect;
        }
        else
        {
            return new Dictionary<Item, uint>();
        }

    }

    public Dictionary<Item, uint> CollectOutputs(bool collectAll, Dictionary<Item, uint> itemsToCollect)
    {
        if (collectAll)
        {
            Dictionary<Item, uint> returnItems = new Dictionary<Item, uint>(outputs.GetInventory());
            outputs.Clear();
            return returnItems;
        }
        if (outputs.RemoveItems(itemsToCollect))
        {
            return itemsToCollect;
        }
        else
        {
            return new Dictionary<Item, uint>();
        }
    }

    private bool QueuePrintJob(Item printItem)
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

        // check we have enough of the required inputs
        foreach (string itemName in recipe.inputs.Keys)
        {
            Item item = (Item)Enum.Parse(typeof(Item), itemName);
            if (inputs.GetItemCount(item) < recipe.inputs[itemName])
            {
                return;
            }
        }

        // remove required inputs
        foreach (string itemName in recipe.inputs.Keys)
        {
            Item item = (Item)Enum.Parse(typeof(Item), itemName);
            inputs.RemoveItem(item, recipe.inputs[itemName]);
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

    public void PrinterStop()
    {
        Halt();
        printerQueue.Clear();
    }
}
