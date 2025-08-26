using UnityEngine;

public class RobertPrinterInterface : MonoBehaviour
{
    private RobertInventory inventory;
    private RobertSensors sensors;

    public void Start()
    {
        inventory = GetComponentInParent<RobertInventory>();
        sensors = GetComponentInParent<RobertSensors>();
    }

    public void HandlePrinterFillCommand(PrinterFillCommand fillCommand)
    {
        Debug.LogError("This Command has not been implemented!");
    }

    public void HandlePrinterExecuteCommand(PrinterExecuteCommand executeCommand)
    {
        Debug.LogError("This Command has not been implemented!");
    }

    public void HandlePrinterRetrieveCommand(PrinterRetrieveCommand retrieveCommand)
    {
        Debug.LogError("This Command has not been implemented!");
    }

    public Response HandlePrinterStatusQuery(PrinterStatusQuery printerQuery) {
        if (sensors.CheckForObjectType<Printer>())
        {
            Printer printer = sensors.GetObjectType<Printer>();
            return printer.HandlePrinterStatusQuery(printerQuery);
        }
        else
        {
            return Response.ErrorResponse("No printer near bot!");
        }
    }
}
