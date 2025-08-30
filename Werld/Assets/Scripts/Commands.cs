
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

public class TeleportToMine : BotCommand
{
    new public const string id = "teleport_to_mine";
}

// Bot Queries
public class BusyQuery : BotCommand
{
    new public const string id = "busy_query";
    public float[] rotation { get; set; }
    public bool relative { get; set; }
}
public class BusyQueryResponse : Response
{
    public bool busy { get; set; }
}

public class PositionQuery: BotCommand
{
    new public const string id = "position_query";
}
public class PositionQueryResponse : Response
{
    public float[] position { get; set; }
    public float[] rotation { get; set; }
}

public class SensorQuery: BotCommand 
{
    new public const string id = "sensor_query";
}
public class SensorQueryResponse : Response
{
    public bool[] readings { get; set; }
}

public class ItemQuery : BotCommand
{
    new public const string id = "item_query";
    public string item_name { get; set; }
}
public class ItemQueryResponse : Response
{
    public uint amount { get; set; }
}

public class InventoryQuery : BotCommand
{
    new public const string id = "inventory_query";
}
public class InventoryQueryResponse : Response
{
    public Dictionary<string, uint> inventory;
}

public class MineScanQuery : BotCommand
{
    new public const string id = "mine_scan_query";
}
public class MineScanQueryResponse : Response
{
    public int[,] map;
}

public class BeaconQuery : BotCommand
{
    new public const string id = "beacon_query";
    public bool relative { get; set; }
}
public class BeaconQueryResponse : Response
{
    public string[] beacons;
    public float[,] positions;
}

public class PrinterStatusQuery : BotCommand
{
    new public const string id = "printer_status_query";
}
public class PrinterStatusQueryResponse : Response
{
    public Dictionary<string, uint> inputs { get; set; }
    public Dictionary<string, uint> outputs { get; set; }
    public bool busy;
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
