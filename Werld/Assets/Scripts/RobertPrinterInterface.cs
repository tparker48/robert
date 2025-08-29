using UnityEngine;

public class RobertPrinterInterface : MonoBehaviour
{
    private ItemContainer inventory;
    private RobertSensors sensors;

    private static Response noPrinterError = Response.ErrorResponse("No Printer in Range!");

    public void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensors>();
    }

    public void HandlePrinterFillCommand(PrinterFillCommand fillCommand)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return;

        ItemGroup itemsToAdd = new ItemGroup(fillCommand.items_to_add);
        printer.AddInputsFrom(ref inventory, itemsToAdd);
    }

    public void HandlePrinterRetrieveCommand(PrinterRetrieveCommand retrieveCommand)
    {
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

    public void HandlePrinterQueueJobCommand(PrinterQueueJob executeCommand)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return;

        if (Items.ItemExists(executeCommand.item_to_print))
        {
            Item itemToPrint = Items.Lookup(executeCommand.item_to_print);
            printer.QueuePrintJob(itemToPrint);
        }
        else
        {
            Debug.Log("Invalid Item Name!");
        }
    }

    public void HandlePrinterStopCommand(PrinterStopCommand _)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return;
        printer.Stop();
    }

    public Response HandlePrinterStatusQuery(PrinterStatusQuery printerQuery)
    {
        Printer printer = GetPrinterInRange();
        if (printer == null) return Response.ErrorResponse("No printer near bot!");
        return printer.HandlePrinterStatusQuery(printerQuery);
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
