using UnityEngine;

public class RobertPrinterInterface : MonoBehaviour
{
    private ItemContainer inventory;
    private RobertSensorInterface sensors;

    private static Response noPrinterError = Response.ErrorResponse("No Printer in Range!");

    public void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensorInterface>();
    }

    public void HandlePrinterFill(string rawCmd)
    {
        PrinterFill fillCommand = CommandParser.Parse<PrinterFill>(rawCmd);
        Printer printer = GetPrinterInRange();
        if (printer == null) return;

        ItemGroup itemsToAdd = new ItemGroup(fillCommand.items_to_add);
        printer.AddInputsFrom(ref inventory, itemsToAdd);
    }

    public void HandlePrinterRetrieve(string rawCmd)
    {
        PrinterRetrieve retrieveCommand = CommandParser.Parse<PrinterRetrieve>(rawCmd);
        Printer printer = GetPrinterInRange();
        if (printer == null) return;

        ItemGroup itemsToCollect = new ItemGroup(retrieveCommand.items_to_collect);

        if (retrieveCommand.from_input)
        {
            printer.RemoveInputsTo(retrieveCommand.collect_all, ref inventory, itemsToCollect);
        }
        else
        {
            printer.RemoveOutputsTo(retrieveCommand.collect_all, ref inventory, itemsToCollect);
        }        
    }

    public void HandlePrinterQueueJob(string rawCmd)
    {
        PrinterQueueJob executeCommand= CommandParser.Parse<PrinterQueueJob>(rawCmd);
        Printer printer = GetPrinterInRange();
        if (printer == null) return;

        if (Items.ItemExists(executeCommand.item_to_print))
        {
            Item itemToPrint = Items.Lookup(executeCommand.item_to_print);
            if (itemToPrint != null)
            {
                printer.QueuePrintJob(itemToPrint, executeCommand.quantity);
            }
            
        }
        else
        {
            Debug.Log("Invalid Item Name!");
        }
    }

    public void HandlePrinterStop(string _)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return;
        printer.Stop();
    }

    public Response HandleCheckPrinterStatus(string _)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return Response.ErrorResponse("No printer near bot!");
        return printer.HandleCheckPrinterStatus();
    }

    private Printer GetPrinterInRange()
    {
        Printer printer = null;
        if (!sensors.GetObjectOfType(ref printer))
        {
            Debug.Log("No Printer In Range!");
        }
        return printer;
    }
}
