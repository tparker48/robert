using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class Robert : MonoBehaviour
{
    public int id;

    float ticker = 1.0f;
    float command_delay = 0.02f;
    bool command_running = false;

    RobertSensors sensors;
    ItemContainer inventory;
    RobertDrill drill;
    RobertPlanter planter;
    RobertMotorics motorics;
    RobertBeaconScanner beaconScanner;
    RobertPrinterInterface printerInterface;

    Queue<string> commandQueue = new Queue<string>();

    void Start()
    {
        TCPServer server = FindObjectOfType<TCPServer>();
        if (server == null)
        {
            throw new Exception("No TCPServer object found!");
        }
        server.RegisterNewBot(this);

        motorics = GetComponent<RobertMotorics>();
        sensors = GetComponent<RobertSensors>();
        inventory = GetComponent<ItemContainer>();
        drill = GetComponent<RobertDrill>();
        planter = GetComponent<RobertPlanter>();
        beaconScanner = GetComponentInChildren<RobertBeaconScanner>();
        printerInterface = GetComponent<RobertPrinterInterface>();

        inventory.AddItem(Items.Lookup("Lettuce Seeds"), 10);
    }

    void Update()
    {
        //ticker += Time.deltaTime;
        //if (ticker < command_delay) return;
        command_running = IsCommandRunning();

        if (!command_running)
        {
            string command_data;
            if (commandQueue.TryDequeue(out command_data))
            {
                HandleCommand(command_data);
            }
        }

        ticker = 0.0f;
    }

    public Response OnCommandRecieved(string recieved_data)
    {
        Command obj = JsonConvert.DeserializeObject<Command>(recieved_data);

        if (obj.cmd_id.Contains("_query"))
        {
            return HandleQuery(recieved_data);
        }
        else if (obj.cmd_id == "halt")
        {
            HandleHalt(recieved_data);
            return Response.GenericResponse("Halting...");
        }
        else
        {
            commandQueue.Enqueue(recieved_data);
            return Response.GenericResponse("Command Queued!");
        }
    }

    void HandleCommand(string recieved_data)
    {
        Command cmd = JsonConvert.DeserializeObject<Command>(recieved_data);
        switch (cmd.cmd_id)
        {
            case MoveCommand.id:
                MoveCommand moveCommand = JsonConvert.DeserializeObject<MoveCommand>(recieved_data);
                motorics.HandleMoveCommand(moveCommand);
                break;
            case RotateCommand.id:
                RotateCommand rotateCommand = JsonConvert.DeserializeObject<RotateCommand>(recieved_data);
                motorics.HandleRotateCommand(rotateCommand);
                break;
            case MineCommand.id:
                MineCommand mineCommand = JsonConvert.DeserializeObject<MineCommand>(recieved_data);
                drill.HandleMineCommand(mineCommand);
                break;
            case PlantCommand.id:
                PlantCommand plantCommand = JsonConvert.DeserializeObject<PlantCommand>(recieved_data);
                planter.HandlePlantCommand(plantCommand);
                break;
            case PrinterFillCommand.id:
                PrinterFillCommand fillCommand = JsonConvert.DeserializeObject<PrinterFillCommand>(recieved_data);
                printerInterface.HandlePrinterFillCommand(fillCommand);
                break;
            case PrinterQueueJob.id:
                PrinterQueueJob executeCommand = JsonConvert.DeserializeObject<PrinterQueueJob>(recieved_data);
                printerInterface.HandlePrinterQueueJobCommand(executeCommand);
                break;
            case PrinterRetrieveCommand.id:
                PrinterRetrieveCommand retreiveCommand = JsonConvert.DeserializeObject<PrinterRetrieveCommand>(recieved_data);
                printerInterface.HandlePrinterRetrieveCommand(retreiveCommand);
                break;
            case PrinterStopCommand.id:
                PrinterStopCommand stopCommand = JsonConvert.DeserializeObject<PrinterStopCommand>(recieved_data);
                printerInterface.HandlePrinterStopCommand(stopCommand);
                break;
            default:
                Debug.Log("Unrecognized cmd_id");
                break;
        }
    }

    Response HandleQuery(string recieved_data)
    {
        Command query = JsonConvert.DeserializeObject<Command>(recieved_data);
        switch (query.cmd_id)
        {
            case BusyQuery.id:
                BusyQueryResponse busy_response = new BusyQueryResponse();
                busy_response.busy = commandQueue.Count != 0 || command_running;
                return busy_response;
            case PositionQuery.id:
                PositionQueryResponse position_response = new PositionQueryResponse();
                position_response.position = new float[3] {
                    transform.position.x,
                    transform.position.y,
                    transform.position.z
                };
                position_response.rotation = new float[3] {
                    transform.rotation.eulerAngles.x,
                    transform.rotation.eulerAngles.y,
                    transform.rotation.eulerAngles.z
                };
                return position_response;
            case SensorQuery.id:
                SensorQueryResponse sensor_response = new SensorQueryResponse();
                sensor_response.readings = sensors.CheckSensors();
                return sensor_response;
            case ItemQuery.id:
                ItemQuery item_query = JsonConvert.DeserializeObject<ItemQuery>(recieved_data);

                if (Items.ItemExists(item_query.item_name))
                {
                    Item item = Items.Lookup(item_query.item_name);
                    ItemQueryResponse item_response = new ItemQueryResponse();
                    item_response.amount = inventory.GetItemCount(item);
                    return item_response;
                }
                else {
                    return Response.ErrorResponse($"Item '{item_query.item_name}' does not exist!");
                }

                
            case InventoryQuery.id:
                InventoryQueryResponse inv_response = new InventoryQueryResponse();
                inv_response.inventory = inventory.GetInventory().ToStringKeys();
                return inv_response;
            case MineralQuery.id:
                MineralQuery scanQuery = JsonConvert.DeserializeObject<MineralQuery>(recieved_data);
                if (drill.InMine())
                {
                    MineralQueryResponse scanResponse = new MineralQueryResponse();
                    scanResponse.map = drill.HandleMineralQuery(scanQuery);
                    return scanResponse;
                }
                else
                {
                    return Response.ErrorResponse("Not in mine!");
                }
            case BeaconQuery.id:
                BeaconQuery beaconQuery = JsonConvert.DeserializeObject<BeaconQuery>(recieved_data);
                return beaconScanner.HandleBeaconQuery(beaconQuery);
            case PrinterStatusQuery.id:
                PrinterStatusQuery printerQuery = JsonConvert.DeserializeObject<PrinterStatusQuery>(recieved_data);
                return printerInterface.HandlePrinterStatusQuery(printerQuery);
            default:
                return Response.ErrorResponse("Unrecognized cmd_id");
        }
    }

    void HandleHalt(string recieved_data)
    {
        HaltCommand hlt_cmd = JsonConvert.DeserializeObject<HaltCommand>(recieved_data);

        foreach (RobertTaskExecutor executor in GetComponents<RobertTaskExecutor>())
        {
            executor.Halt();
        }

        if (hlt_cmd.clear_command_buffer)
        {
            commandQueue.Clear();
        }
    }

    private bool IsCommandRunning()
    {
        foreach (RobertTaskExecutor executor in GetComponents<RobertTaskExecutor>())
        {
            if (executor.IsBusy())
            {
                return true;
            }
        }
        return false;
    }

}
