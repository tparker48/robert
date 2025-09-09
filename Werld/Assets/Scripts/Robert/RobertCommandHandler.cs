using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public delegate void CommandDelegate(string rawCmd);
public delegate Response QueryDelegate(string rawCmd);

public static class CommandParser {
    public static T Parse<T>(string rawTxt)
    {
        return JsonConvert.DeserializeObject<T>(rawTxt);
    }
}

public class RobertCommandHandler : MonoBehaviour
{
    RobertSensorInterface sensorInterface;
    RobertMineInterface mineInterface;
    RobertGrowboxInterface growboxInterface;
    RobertMotoricsInterface motoricsInterface;
    RobertBeaconInterface beaconInterface;
    RobertPrinterInterface printerInterface;
    RobertInventoryInterface inventoryInterface;
    RobertUpgradeInterface upgradeInterface;
    RobertBuildInterface buildInterface;

    public Dictionary<string, CommandDelegate> commandMap;
    public Dictionary<string, QueryDelegate> queryMap;

    // Start is called before the first frame update
    void Start()
    {
        sensorInterface = GetComponent<RobertSensorInterface>();
        motoricsInterface = GetComponent<RobertMotoricsInterface>();
        mineInterface = GetComponent<RobertMineInterface>();
        growboxInterface = GetComponent<RobertGrowboxInterface>();
        beaconInterface = GetComponentInChildren<RobertBeaconInterface>();
        printerInterface = GetComponent<RobertPrinterInterface>();
        inventoryInterface = GetComponent<RobertInventoryInterface>();
        upgradeInterface = GetComponent<RobertUpgradeInterface>();
        buildInterface = GetComponent<RobertBuildInterface>();

        commandMap = new Dictionary<string, CommandDelegate>
        {
            { Move.id,  new CommandDelegate(motoricsInterface.HandleMove) },
            { Rotate.id,  new CommandDelegate(motoricsInterface.HandleRotate) },
            { PlantSeed.id,  new CommandDelegate(growboxInterface.HandlePlantSeed) },
            { Harvest.id,  new CommandDelegate(growboxInterface.HandleHarvest) },
            { PrinterFill.id,  new CommandDelegate(printerInterface.HandlePrinterFill) },
            { PrinterQueueJob.id,  new CommandDelegate(printerInterface.HandlePrinterQueueJob) },
            { PrinterRetrieve.id,  new CommandDelegate(printerInterface.HandlePrinterRetrieve) },
            { PrinterStop.id,  new CommandDelegate(printerInterface.HandlePrinterStop) },
            { DepositToStorage.id,  new CommandDelegate(inventoryInterface.HandleDepositCommand) },
            { WithdrawFromStorage.id,  new CommandDelegate(inventoryInterface.HandleWithdrawCommand) },
            { DepositToRobert.id,  new CommandDelegate(inventoryInterface.HandleDepositRobertCommand) },
            { WithdrawFromRobert.id,  new CommandDelegate(inventoryInterface.HandleWithdrawRobertCommand) },
            { SellItem.id,  new CommandDelegate(inventoryInterface.HandleSellItem) },
            { Teleport.id,  new CommandDelegate(mineInterface.HandleTeleport) },
            { TeleportReturn.id,  new CommandDelegate(mineInterface.HandleTeleportReturn) },
            { RefreshTeleporter.id,  new CommandDelegate(mineInterface.HandleRefreshTeleporter) },
            { Mine.id,  new CommandDelegate(mineInterface.HandleMine) },
            { BuildRoomEquipment.id,  new CommandDelegate(buildInterface.HandleBuildRoomEquipment) },
            { CreateBeacon.id,  new CommandDelegate(beaconInterface.HandleCreateBeacon) },
            { DeleteBeacon.id,  new CommandDelegate(beaconInterface.HandleDeleteBeacon) },
            { UseElevator.id,  new CommandDelegate(motoricsInterface.HandleUseElevator) },
            { Upgrade.id,  new CommandDelegate(upgradeInterface.HandleUpgrade) },
        };

        queryMap = new Dictionary<string, QueryDelegate>
        {
            { GetPosition.id,  new QueryDelegate(motoricsInterface.HandleGetPosition) },
            { CheckSensors.id,  new QueryDelegate(sensorInterface.HandleCheckSensors) },
            { GetItemCount.id,  new QueryDelegate(inventoryInterface.HandleGetItemCount) },
            { GetFullInventory.id,  new QueryDelegate(inventoryInterface.HandleGetFullInventory) },
            { ScanMine.id,  new QueryDelegate(mineInterface.HandleScanMine) },
            { ScanBeacons.id,  new QueryDelegate(beaconInterface.HandleScanBeacons) },
            { CheckPrinterStatus.id,  new QueryDelegate(printerInterface.HandleCheckPrinterStatus) },
            { GetFloor.id,  new QueryDelegate(motoricsInterface.HandleGetShipFloor) },
            { CheckGrowBoxStatus.id,  new QueryDelegate(growboxInterface.HandleCheckGrowBoxStatus) },
            { CheckSellValue.id,  new QueryDelegate(inventoryInterface.HandleCheckSellValue) },
            { GetUpgradeCost.id, new QueryDelegate(upgradeInterface.HandleGetUpgradeRequirements) },
            { GetBotType.id, new QueryDelegate(upgradeInterface.HandleGetBotType) }
        };
    }

    private void NotImplemented(string _)
    {
        throw new NotImplementedException();
    }

    public void RunCommand(string rawCmd)
    {
        BotCommand cmd = CommandParser.Parse<BotCommand>(rawCmd);
        if (commandMap.ContainsKey(cmd.cmd_id))
        {
            commandMap[cmd.cmd_id](rawCmd);
        }
        else
        {
            Debug.Log("Unrecognized cmd_id");
        }
    }

    public Response RunQuery(string rawCmd)
    {
        BotCommand cmd = CommandParser.Parse<BotCommand>(rawCmd);
        if (queryMap.ContainsKey(cmd.cmd_id))
        {
            return queryMap[cmd.cmd_id](rawCmd);
        }
        else
        {
            return Response.ErrorResponse("Unrecognized cmd_id");
        }
    }
}


