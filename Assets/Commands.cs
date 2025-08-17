using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command {
    public const string id = "";
    public string cmd_id { get; set; }
}

// COMMANDS
public class MoveCommand: Command {
    public const string id = "move";
    public float[] position { get; set; }
    public bool relative {get; set; }
}

public class RotateCommand : Command
{
    public const string id = "rotate";
    public float angle { get; set; }
    public bool relative { get; set; }
}

public class HaltCommand : Command
{
    public const string id = "halt";
    public bool clear_command_buffer { get; set; }
}


// QUERIES
public class BusyQuery : Command
{
    public const string id = "busy_query";
    public float[] rotation { get; set; }
    public bool relative { get; set; }
}
public class BusyQueryResponse {
    public bool busy { get; set; }
}

public class PositionQuery: Command {
    public const string id = "position_query";
}
public class PositionQueryResponse {
    public float[] position { get; set; }
    public float[] rotation { get; set; }
}