public class Command {
    public const string id = "";
    public int bot_id { get; set; }
    public string cmd_id { get; set; }
}

// COMMANDS
public class MoveCommand: Command {
    new public const string id = "move";
    public float[] position { get; set; }
    public bool relative {get; set; }
}

public class RotateCommand : Command
{
    new public const string id = "rotate";
    public float angle { get; set; }
    public bool relative { get; set; }
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
public class BusyQueryResponse {
    public bool busy { get; set; }
}

public class PositionQuery: Command {
    new public const string id = "position_query";
}
public class PositionQueryResponse {
    public float[] position { get; set; }
    public float[] rotation { get; set; }
}

public class SensorQuery: Command {
    new public const string id = "sensor_query";
}
public class SensorQueryResponse {
    public bool[] readings { get; set; } 
}