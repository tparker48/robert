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
    RobertStorageInterface storageInterface;

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
        storageInterface = GetComponent<RobertStorageInterface>();

        GetComponentInChildren<Beacon>().beaconName = "robert " + id;
        GetComponentInChildren<Label>().text = "rob_" + id;
        GetComponentInChildren<Label>().textColor = Color.white;

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

        string taskPercentOverlayKey = $"robert_{id}:task_progress";
        if (command_running)
        {
            float percentComplete = 0.0f;
            foreach (RobertTaskExecutor ex in GetComponents<RobertTaskExecutor>())
            {
                if (ex.IsBusy())
                {
                    percentComplete = Mathf.Round(100.0f * ex.PercentComplete());
                    HoverText.Instance.OverlayText(taskPercentOverlayKey, $"[{ex.GetBusyText()}: {percentComplete}%]", transform.position, .75f, .75f);
                    HoverText.Instance.SetOverlayTextColor(taskPercentOverlayKey, Color.green);
                }
            }
            
        }
        else
        {
            HoverText.Instance.RemoveOverlay(taskPercentOverlayKey);
        }

        ticker = 0.0f;

    }

    public Response OnCommandRecieved(string recieved_data)
    {
        BotCommand obj = JsonConvert.DeserializeObject<BotCommand>(recieved_data);

        if (obj.is_query)
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
        BotCommand cmd = JsonConvert.DeserializeObject<BotCommand>(recieved_data);
        Debug.Log("Recieved Command: " + cmd.cmd_id);
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
            case HarvestCommand.id:
                HarvestCommand harvestCommand = JsonConvert.DeserializeObject<HarvestCommand>(recieved_data);
                planter.HandleHarvestCommand(harvestCommand);
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
            case DepositToStorageCommand.id:
                DepositToStorageCommand depositCommand = JsonConvert.DeserializeObject<DepositToStorageCommand>(recieved_data);
                storageInterface.HandleDepositCommand(depositCommand);
                break;
            case WithdrawFromStorageCommand.id:
                WithdrawFromStorageCommand withdrawCommand = JsonConvert.DeserializeObject<WithdrawFromStorageCommand>(recieved_data);
                storageInterface.HandleWithdrawCommand(withdrawCommand);
                break;
            case MineTeleportComamnd.id:
                Teleporter teleporter = null;
                if (sensors.GetObjectOfType(ref teleporter))
                {
                    teleporter.TeleportRobertToMine(this);
                }
                break;
            case MineReturnCommand.id:
                if (drill.IsInMine())
                {
                    Cave.Instance.ReturnBot(this);
                }
                break;
            case SellItemCommand.id:
                SellItemCommand sellCmd = new SellItemCommand();
                Tradepost tradepost = null;
                if (sensors.GetObjectOfType(ref tradepost))
                {
                    tradepost.HandleSellCommand(ref inventory, sellCmd);
                }
                break;
            default:
                Debug.Log("Unrecognized cmd_id");
                break;
        }
    }

    Response HandleQuery(string recieved_data)
    {
        BotCommand query = JsonConvert.DeserializeObject<BotCommand>(recieved_data);
        Debug.Log("Recieved Query: " + query.cmd_id);
        switch (query.cmd_id)
        {
            case CheckBusy.id:
                CheckBusyResponse busy_response = new CheckBusyResponse();
                busy_response.busy = commandQueue.Count != 0 || command_running;
                return busy_response;
            case GetPosition.id:
                GetPositionResponse position_response = new GetPositionResponse();
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
            case CheckSensors.id:
                CheckSensorsResponse sensor_response = new CheckSensorsResponse();
                sensor_response.readings = sensors.CheckSensors();
                return sensor_response;
            case GetItemCount.id:
                GetItemCount item_query = JsonConvert.DeserializeObject<GetItemCount>(recieved_data);

                if (Items.ItemExists(item_query.item_name))
                {
                    Item item = Items.Lookup(item_query.item_name);
                    GetItemCountResponse item_response = new GetItemCountResponse();
                    item_response.amount = inventory.GetItemCount(item);
                    return item_response;
                }
                else
                {
                    return Response.ErrorResponse($"Item '{item_query.item_name}' does not exist!");
                }
            case GetFullInventory.id:
                GetFullInventoryResponse inv_response = new GetFullInventoryResponse();
                inv_response.inventory = inventory.GetInventory().ToStringKeys();
                return inv_response;
            case ScanMine.id:
                ScanMine scanQuery = JsonConvert.DeserializeObject<ScanMine>(recieved_data);
                if (drill.IsInMine())
                {
                    ScanMineResponse scanResponse = new ScanMineResponse();
                    scanResponse.scan_output_path = drill.HandleScanMine(scanQuery);
                    return scanResponse;
                }
                else
                {
                    return Response.ErrorResponse("Not in mine!");
                }
            case ScanBeacons.id:
                ScanBeacons scanBeacons = JsonConvert.DeserializeObject<ScanBeacons>(recieved_data);
                return beaconScanner.HandleScanBeacons(scanBeacons);
            case CheckPrinterStatus.id:
                CheckPrinterStatus printerQuery = JsonConvert.DeserializeObject<CheckPrinterStatus>(recieved_data);
                return printerInterface.HandleCheckPrinterStatus(printerQuery);
            case GetFloor.id:
                GetFloorResponse shipFloorResponse = new GetFloorResponse();
                shipFloorResponse.floor = Ship.GetFloor(transform.position);
                return shipFloorResponse;
            case CheckGrowBoxStatus.id:
                CheckGrowBoxStatus plantQuery = JsonConvert.DeserializeObject<CheckGrowBoxStatus>(recieved_data);
                return planter.HandleCheckGrowBoxStatus(plantQuery);
            case CheckSellValue.id:
                CheckSellValue sellValueQuery = new CheckSellValue();
                Tradepost tradepost = null;
                if (sensors.GetObjectOfType(ref tradepost))
                {
                    return tradepost.HandleCheckSellValue(sellValueQuery);
                }
                else
                {
                    return Response.ErrorResponse("Not at a tradepost!");
                }
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
