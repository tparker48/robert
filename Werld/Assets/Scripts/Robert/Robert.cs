using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class Robert : MonoBehaviour
{
    public int id;
    Queue<string> commandQueue = new Queue<string>();
    bool command_running = false;

    public ItemContainer inventory;
    public RobertTraits traits;

    RobertCommandHandler commandHandler;


    void Start()
    {
        commandHandler = GetComponent<RobertCommandHandler>();
        inventory = GetComponent<ItemContainer>();
        traits = GetComponent<RobertTraits>();

        TCPServer server = FindObjectOfType<TCPServer>();
        if (server == null)
        {
            throw new Exception("No TCPServer object found!");
        }
        server.RegisterNewBot(this);

        GetComponentInChildren<Beacon>().beaconName = "robert " + id;
        GetComponentInChildren<Label>().text = "rob_" + id;
        GetComponentInChildren<Label>().textColor = Color.white;
    }

    void Update()
    {
        command_running = IsCommandRunning();

        if (!command_running)
        {
            string cmdData;
            if (commandQueue.TryDequeue(out cmdData))
            {
                commandHandler.RunCommand(cmdData);
            }
        }

        string taskPercentOverlayKey = $"robert_{id}:task_progress";
        if (command_running)
        {
            float percentComplete;
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
    }

    public void InitType(RobertType robType, int level=0)
    {
        traits.SetRobertType(robType);
        traits.SetLevel(level);
    }

    public Response OnCommandRecieved(string cmdData)
    {
        BotCommand obj = JsonConvert.DeserializeObject<BotCommand>(cmdData);

        if (obj.is_query)
        {
            return RunQuery(cmdData);
        }
        else if (obj.cmd_id == "halt")
        {
            HandleHalt(cmdData);
            return Response.GenericResponse("Halting...");
        }
        else
        {
            commandQueue.Enqueue(cmdData);
            return Response.GenericResponse("Command Queued!");
        }
    }

    Response RunQuery(string cmdData)
    {
        if (JsonConvert.DeserializeObject<BotCommand>(cmdData).cmd_id == CheckBusy.id) {
            CheckBusyResponse busy_response = new CheckBusyResponse();
            busy_response.busy = commandQueue.Count != 0 || command_running;
            return busy_response;
        }     
        else {
            return commandHandler.RunQuery(cmdData);
        }
    }

    void HandleHalt(string cmdData)
    {
        Halt hlt_cmd = JsonConvert.DeserializeObject<Halt>(cmdData);

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
