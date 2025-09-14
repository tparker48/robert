
using System.Collections.Generic;

public class Command
{
    public const string id = "";
    public string cmd_id { get; set; }
    public bool ship_command { get; set; }
}

public class Response
{
    public string message { get; set; }
    public bool error { get; set; }

    public static Response GenericResponse(string message)
    {
        Response response = new Response();
        response.message = message;
        response.error = false;
        return response;
    }

    public static Response ErrorResponse(string message)
    {
        Response response = new Response();
        response.message = message;
        response.error = true;
        return response;
    }
}

// BOT COMMANDS
public class BotCommand : Command
{
    public int bot_id { get; set; }
}

public class Move : BotCommand
{
    new public const string id = "move";
    public float[] position { get; set; }
    public bool relative { get; set; }
}

public class Rotate : BotCommand
{
    new public const string id = "rotate";
    public float angle { get; set; }
    public bool relative { get; set; }
}

public class Mine : BotCommand
{
    new public const string id = "mine";
    public string direction { get; set; }
}

public class PlantSeed : BotCommand
{
    new public const string id = "plant";
    public string seed_item { get; set; }
}

public class Harvest : BotCommand
{
    new public const string id = "harvest";
}

public class PrinterFill : BotCommand
{
    new public const string id = "printer_fill";
    public Dictionary<string, uint> items_to_add { get; set; }
}

public class PrinterQueueJob : BotCommand
{
    new public const string id = "printer_queue_job";
    public string item_to_print { get; set; }
    public int quantity { get; set; }
}

public class PrinterStop : BotCommand
{
    new public const string id = "printer_stop";
}

public class PrinterRetrieve : BotCommand
{
    new public const string id = "printer_retrieve";
    public bool from_input { get; set; }
    public bool collect_all { get; set; }
    public Dictionary<string, uint> items_to_collect { get; set; }
}

public class SellItem : BotCommand
{
    new public const string id = "sell";
    public string item { get; set; }
    public uint quantity { get; set; }
}

public class Halt : BotCommand
{
    new public const string id = "halt";
    public bool clear_command_buffer { get; set; }
}

public class DepositToStorage : BotCommand
{
    new public const string id = "deposit_to_storage";
    public Dictionary<string, uint> items_to_deposit { get; set; }
}

public class WithdrawFromStorage : BotCommand
{
    new public const string id = "withdraw_from_storage";
    public Dictionary<string, uint> items_to_withdraw { get; set; }
}

public class DepositToRobert : BotCommand
{
    new public const string id = "deposit_to_robert";
    public int other_robert_id { get; set; }
    public Dictionary<string, uint> items_to_deposit { get; set; }
}

public class WithdrawFromRobert : BotCommand
{
    new public const string id = "withdraw_from_robert";
    public int other_robert_id { get; set; }
    public Dictionary<string, uint> items_to_withdraw { get; set; }
}

public class RefreshTeleporter : BotCommand
{
    new public const string id = "refresh_teleporter";
}

public class Teleport : BotCommand
{
    new public const string id = "teleport";
}

public class TeleportReturn : BotCommand
{
    new public const string id = "teleport_return";
}

public class BuildRoomEquipment : BotCommand
{
    new public const string id = "build_room_equipment";
    public string equipment { get; set; }
}

public class CreateBeacon : BotCommand
{
    new public const string id = "create_beacon";
    public string beacon_name { get; set; }
}

public class DeleteBeacon : BotCommand
{
    new public const string id = "delete_beacon";
    public string beacon_name { get; set; }
}

public class Upgrade : BotCommand
{
    new public const string id = "upgrade";
}

public class UseElevator : BotCommand
{
    new public const string id = "use_elevator";
    public int floor { get; set; }
}

// Bot Queries
public class CheckBusy : BotCommand
{
    new public const string id = "check_busy";
}
public class CheckBusyResponse : Response
{
    public bool busy { get; set; }
}

public class GetPosition: BotCommand
{
    new public const string id = "get_position";
}
public class GetPositionResponse : Response
{
    public float[] position { get; set; }
    public float[] rotation { get; set; }
}

public class CheckSensors: BotCommand 
{
    new public const string id = "check_sensors";
}
public class CheckSensorsResponse : Response
{
    public bool[] readings { get; set; }
}

public class GetItemCount : BotCommand
{
    new public const string id = "get_item_count";
    public string item_name { get; set; }
}
public class GetItemCountResponse : Response
{
    public uint amount { get; set; }
}

public class GetFullInventory : BotCommand
{
    new public const string id = "get_full_inventory";
}
public class GetFullInventoryResponse : Response
{
    public Dictionary<string, uint> inventory;
}

public class ScanMine : BotCommand
{
    new public const string id = "scan_mine";
}
public class ScanMineResponse : Response
{
    public string scan_output_path { get; set; }
}

public class ScanBeacons : BotCommand
{
    new public const string id = "scan_beacons";
    public bool relative { get; set; }
}
public class ScanBeaconsResponse : Response
{
    public Dictionary<string, float[]> beacons;
}

public class CheckPrinterStatus : BotCommand
{
    new public const string id = "check_printer_status";
}
public class CheckPrinterStatusResponse : Response
{
    public Dictionary<string, uint> inputs { get; set; }
    public Dictionary<string, uint> outputs { get; set; }
    public bool busy;
}

public class GetFloor : BotCommand
{
    new public const string id = "get_floor";
}
public class GetFloorResponse : Response
{
    public int floor { get; set; }
}

public class CheckGrowBoxStatus : BotCommand
{
    new public const string id = "check_growbox_status";
}
public class CheckGrowBoxStatusResponse : Response
{
    public bool grow_box_in_range { get; set; }
    public bool has_plant { get; set; }
    public string plant_name { get; set; }
    public bool ready_to_harvest { get; set; }
    public float plant_age_seconds { get; set; }
    public float[] plant_phase_times { get; set; }
}

public class CheckSellValue : BotCommand
{
    new public const string id = "check_sell_value";
    public string item { get; set; }
}
public class CheckSellValueResponse : Response
{
    public bool valid_item { get; set; }
    public int sell_value { get; set; }
}

public class GetUpgradeCost : BotCommand
{
    new public const string id = "get_upgrade_cost";
}
public class GetUpgradeCostResponse : Response
{
    public Dictionary<string, uint> items { get; set;}
}

public class GetBotType : BotCommand
{
    new public const string id = "get_type";
}

public class GetBotTypeResponse : Response
{
    public string type { get; set; }
}

// SHIP COMMANDS
public class ShipCommand : Command
{
}

public class BuyRobert : ShipCommand
{
    new public const string id = "ship_buy_robert";
    public string type { get; set; }
}

public class GetRobertCost : ShipCommand
{
    new public const string id = "ship_get_robert_cost";
    public string type { get; set; }
}
public class GetRobertCostResponse : Response
{
    public int cost { get; set; }
}

public class AddFloor : ShipCommand
{
    new public const string id = "ship_add_floor";
}

public class GetBits : ShipCommand
{
    new public const string id = "ship_get_bits";
}
public class GetBitsResponse : Response
{
    public uint bits { get; set; }
}

public class GetNewFloorCost : ShipCommand
{
    new public const string id = "ship_get_new_floor_cost";
}
public class GetNewFloorCostResponse : Response
{
    public uint cost { get; set; }
}

public class GetEquipmentCost : ShipCommand
{
    new public const string id = "get_equipment_cost";
    public string equipment { get; set; }
}
public class GetEquipmentCostResponse : Response
{
    public Dictionary<string, uint> itemsNeeded { get; set; }
}

public class GetNumFloors : ShipCommand
{
    new public const string id = "get_num_floors";
}
public class GetNumFloorsResponse : Response
{
    public uint floors { get; set; }
}