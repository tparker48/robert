using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class RobertCommandProcessor : MonoBehaviour
{
    public int id;

    float ticker = 1.0f;
    float command_delay = 0.02f;
    bool command_running = false;

    RobertMotorics motorics;
    RobertSensors sensors;

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
    }

    void Update()
    {
        ticker += Time.deltaTime;
        if (ticker < command_delay) return;

        command_running = motorics.inMotion();

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

    public string OnCommandRecieved(string recieved_data)
    {
        Command obj = JsonConvert.DeserializeObject<Command>(recieved_data);

        if (obj.cmd_id.Contains("_query"))
        {
            return HandleQuery(recieved_data);
        }
        else if (obj.cmd_id == "halt")
        {
            HandleHalt(recieved_data);
            return "Halting!";
        }
        else
        {
            commandQueue.Enqueue(recieved_data);
            return "Command Queued!";
        }
    }

    void HandleCommand(string recieved_data)
    {
        Command cmd = JsonConvert.DeserializeObject<Command>(recieved_data);
        switch (cmd.cmd_id)
        {
            case MoveCommand.id:
                MoveCommand mv_cmd = JsonConvert.DeserializeObject<MoveCommand>(recieved_data);
                motorics.HandleMoveCommand(mv_cmd);
                break;
            case RotateCommand.id:
                RotateCommand rot_cmd = JsonConvert.DeserializeObject<RotateCommand>(recieved_data);
                motorics.HandleRotateCommand(rot_cmd);
                break;
            default:
                Debug.Log("Unrecognized cmd_id");
                break;
        }
    }

    string HandleQuery(string recieved_data)
    {
        Command query = JsonConvert.DeserializeObject<Command>(recieved_data);
        switch (query.cmd_id)
        {
            case BusyQuery.id:
                BusyQueryResponse busy_response = new BusyQueryResponse();
                busy_response.busy = commandQueue.Count != 0 || command_running;
                return JsonConvert.SerializeObject(busy_response);
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
                return JsonConvert.SerializeObject(position_response);
            case SensorQuery.id:
                SensorQueryResponse sensor_response = new SensorQueryResponse();
                sensor_response.readings = sensors.CheckSensors();
                return JsonConvert.SerializeObject(sensor_response);
            default:
                return "Unrecognized cmd_id";
        }
    }

    void HandleHalt(string recieved_data) {
        HaltCommand hlt_cmd = JsonConvert.DeserializeObject<HaltCommand>(recieved_data);

        // cancel any active commands
        motorics.Halt();

        if (hlt_cmd.clear_command_buffer)
        {
            commandQueue.Clear();
        }
    }
    
}
