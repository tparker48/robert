using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// rough sketch
// printer has a little input inventory you can put stuff in
// print has an output inventory where stuff is produced
// bots fill inputs, start a PRINT command, and then pick up outputs
// if there's no valid recipe for the given inputs, nothing happens
// when somethign gets printed, there's an output zone and some visual indication 
// the bot must be at the output zone to pickup the output
// no printing if the output zone is still occupied
// functions needed to clear out current inputs
// functions needed to check what the output will be given the current inputs
// some flat processing time to make any recipe

public class Printer : MonoBehaviour
{
    private uint inputCapacity = 24;
    private uint inputCount = 0;
    private uint printerLevel = 1;
    private Dictionary<Item, uint> inputs = new Dictionary<Item, uint>();
    private Dictionary<Item, uint> outputs = new Dictionary<Item, uint>();


    public bool debugAddBitToInputs = false;
    public bool debugTryPrint = false;
    public bool debugPrintInput = false;
    public bool debugPrintOutput = false;
    public bool debugClearOutput = false;
    public void Update()
    {
        if (debugAddBitToInputs)
        {
            debugAddBitToInputs = false;
            AddInput(Item.Bits, 1);
        }

        if (debugTryPrint)
        {
            debugTryPrint = false;
            Print(Item.Bits);
        }

        if (debugPrintInput)
        {
            debugPrintInput = false;
            foreach (Item item in inputs.Keys)
            {
                Debug.Log(item.ToString() + " " + inputs[item]);
            }
        }

        if (debugPrintOutput)
        {
            debugPrintOutput = false;
            foreach (Item item in outputs.Keys)
            {
                Debug.Log(item.ToString() + " " + outputs[item]);
            }
        }

        if (debugClearOutput)
        {
            debugClearOutput = false;
            CollectOutputs();
        }


    }

    public void AddInput(Item item, uint amount)
    {
        // can't fill beyond capacity
        if (inputCount + amount > inputCapacity)
        {
            return;
        }

        if (inputs.ContainsKey(item))
        {
            inputs[item] += amount;
        }
        else
        {
            inputs[item] = amount;
        }

        inputCount += amount;
    }

    public Dictionary<Item, uint> CollectInputs()
    {
        Dictionary<Item, uint> returnItems = new Dictionary<Item, uint>(inputs);
        inputs.Clear();
        inputCount = 0;
        return returnItems;
    }

    public Dictionary<Item, uint> CollectOutputs()
    {
        Dictionary<Item, uint> returnItems = new Dictionary<Item, uint>(outputs);
        outputs.Clear();
        return returnItems;
    }

    public bool Print(Item printItem)
    {
        if (outputs.Count != 0)
        {
            return false;
        }

        if (PrinterRecipes.RecipeExists(printItem))
        {
            Recipe recipe = PrinterRecipes.GetRecipe(printItem);

            // check we have enough of the required inputs
            foreach (string itemName in recipe.inputs.Keys)
            {
                Item item = (Item)Enum.Parse(typeof(Item), itemName);
                if (!inputs.ContainsKey(item) || inputs[item] < recipe.inputs[itemName])
                {
                    return false;
                }
            }

            // remove required inputs
            foreach (string itemName in recipe.inputs.Keys)
            {
                Item item = (Item)Enum.Parse(typeof(Item), itemName);
                inputs[item] -= recipe.inputs[itemName];
                if (inputs[item] == 0)
                {
                    inputs.Remove(item);
                }
            }
            outputs = new Dictionary<Item, uint> { { printItem, recipe.quantity } };
            return true;
        }
        return false;
    }
}
