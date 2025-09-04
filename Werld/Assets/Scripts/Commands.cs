
using System.Collections.Generic;

public class Command
{
    public const string id = "";
    public string cmd_id { get; set; }
    public bool ship_command { get; set; }
    public bool is_query { get; set; }
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

public class MoveCommand : BotCommand
{
    new public const string id = "move";
    public float[] position { get; set; }
    public bool relative { get; set; }
}

public class RotateCommand : BotCommand
{
    new public const string id = "rotate";
    public float angle { get; set; }
    public bool relative { get; set; }
}

public class MineCommand : BotCommand
{
    new public const string id = "mine";
    public string direction { get; set; }
}

public class PlantCommand : BotCommand
{
    new public const string id = "plant";
    public string seed_item { get; set; }
}

public class HarvestCommand : BotCommand
{
    new public const string id = "harvest";
}

public class PrinterFillCommand : BotCommand
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

public class PrinterStopCommand : BotCommand
{
    new public const string id = "printer_stop";
}

public class PrinterRetrieveCommand : BotCommand
{
    new public const string id = "printer_retrieve";
    public bool from_input { get; set; }
    public bool collect_all { get; set; }
    public Dictionary<string, uint> items_to_collect { get; set; }
}

public class SellItemCommand : BotCommand
{
    new public const string id = "sell";
    public string item { get; set; }
    public uint quantity { get; set; }
}

public class HaltCommand : BotCommand
{
    new public const string id = "halt";
    public bool clear_command_buffer { get; set; }
}

public class DepositToStorageCommand : BotCommand
{
    new public const string id = "deposit_to_storage";
    public Dictionary<string, uint> items_to_deposit { get; set; }
}

public class WithdrawFromStorageCommand : BotCommand
{
    new public const string id = "withdraw_from_storage";
    public Dictionary<string, uint> items_to_withdraw { get; set; }
}

public class MineTeleportComamnd : BotCommand
{
    new public const string id = "mine_teleport";
}
public class MineReturnCommand : BotCommand
{
    new public const string id = "mine_return";
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
    public int sell_value{ get; set; }
}

// SHIP COMMANDS
public class ShipCommand : Command
{
}

public class ShipBitsQuery : ShipCommand
{
    new public const string id = "ship_bits_query";
}
public class ShipBitsQueryResponse : Response
{
    public uint bits;
}
