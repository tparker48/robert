
using System.Collections.Generic;

public class Command
{
    public const string id = "";
    public int bot_id { get; set; }
    public string cmd_id { get; set; }
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

// COMMANDS
public class MoveCommand : Command
{
    new public const string id = "move";
    public float[] position { get; set; }
    public bool relative { get; set; }
}

public class RotateCommand : Command
{
    new public const string id = "rotate";
    public float angle { get; set; }
    public bool relative { get; set; }
}

public class MineCommand : Command
{
    new public const string id = "mine";
    public string direction { get; set; }
}

public class PlantCommand : Command
{
    new public const string id = "plant";
    public int seed_item_id { get; set; }
}

public class PrinterFillCommand : Command
{
    new public const string id = "printer_fill";
    public Dictionary<string, uint> items_to_add { get; set; }
}

public class PrinterQueueJob : Command
{
    new public const string id = "printer_queue_job";
    public string item_to_print { get; set; }
    public int quantity { get; set; }
}

public class PrinterStopCommand : Command
{
    new public const string id = "printer_stop";
}

public class PrinterRetrieveCommand : Command
{
    new public const string id = "printer_retrieve";
    public bool from_input { get; set; }
    public bool collect_all { get; set; }
    public Dictionary<string, uint> items_to_collect { get; set; }
}

public class HaltCommand : Command
{
    new public const string id = "halt";
    public bool clear_command_buffer { get; set; }
}

// QUERIES
public class BusyQuery : Command
{
    new public const string id = "busy_query";
    public float[] rotation { get; set; }
    public bool relative { get; set; }
}
public class BusyQueryResponse : Response
{
    public bool busy { get; set; }
}

public class PositionQuery: Command
{
    new public const string id = "position_query";
}
public class PositionQueryResponse : Response
{
    public float[] position { get; set; }
    public float[] rotation { get; set; }
}

public class SensorQuery: Command 
{
    new public const string id = "sensor_query";
}
public class SensorQueryResponse : Response
{
    public bool[] readings { get; set; }
}

public class ItemQuery : Command
{
    new public const string id = "item_query";
    public string item_name { get; set; }
}
public class ItemQueryResponse : Response
{
    public uint amount { get; set; }
}

public class InventoryQuery : Command
{
    new public const string id = "inventory_query";
}
public class InventoryQueryResponse : Response
{
    public Dictionary<string, uint> inventory;
}

public class MineralQuery : Command
{
    new public const string id = "mineral_query";
}
public class MineralQueryResponse : Response
{
    public int[,] map;
}

public class BeaconQuery : Command
{
    new public const string id = "beacon_query";
    public bool relative { get; set; }
}
public class BeaconQueryResponse : Response
{
    public string[] beacons;
    public float[,] positions;
}

public class PrinterStatusQuery : Command
{
    new public const string id = "printer_status_query";
}
public class PrinterStatusQueryResponse : Response
{
    public Dictionary<string, uint> inputs { get; set; }
    public Dictionary<string, uint> outputs { get; set; }
    public bool busy;
}
