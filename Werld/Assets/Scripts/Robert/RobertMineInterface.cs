using System.IO;
using UnityEngine;

public class RobertMineInterface : RobertTimedTaskExecutor<Mine>
{
    private float mineSpeed = 1.0f;
    public int scanRadius = 35;
    private bool isInMine = false;

    private Vector3 mineTarget;
    private ItemContainer inventory;
    private RobertSensorInterface sensors;

    void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
        sensors = GetComponentInParent<RobertSensorInterface>();
    }

    void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    public bool IsInMine()
    {
        return isInMine;
    }

    public void SetMineState(bool inMine)
    {
        isInMine = inMine;
    }

    protected override void ExecuteOnTaskEnd(Mine task)
    {
        inventory.AddItems(Cave.Instance.MineCell(mineTarget));
    }

    public void HandleMine(string rawCmd)
    {
        Mine mineCommand = CommandParser.Parse<Mine>(rawCmd);

        mineTarget = transform.position + 1.4f * transform.forward;
        if (mineCommand.direction == "left")
        {
            mineTarget -= transform.right;
        }
        else if (mineCommand.direction == "right")
        {
            mineTarget += transform.right;
        }

        if (Cave.Instance.CanMine(mineTarget))
        {
            float mineTime = Cave.Instance.GetCellToughness(mineTarget) * mineSpeed;
            StartTimedTask(mineCommand, mineTime);
        }
    }

    public Response HandleScanMine(string _)
    {
        if (IsInMine())
        {
            ScanMineResponse scanResponse = new ScanMineResponse();
            scanResponse.scan_output_path = Scan();
            return scanResponse;
        }
        else
        {
            return Response.ErrorResponse("Not in mine!");
        }
    }

    private string Scan()
    {
        string filepath = Application.dataPath + "/tempdata/mine_scan.txt";
        string map = Cave.Instance.Scan(transform.position, scanRadius);
        File.WriteAllText(filepath, map);
        return filepath;
    }


    public void HandleTeleport(string _)
    {
        Teleporter teleporter = null;
        if (sensors.GetObjectOfType(ref teleporter))
        {
            teleporter.TeleportRobertToMine(GetComponentInParent<Robert>());
        }
    }

    public void HandleTeleportReturn(string _)
    {
        if (IsInMine())
        {
            Cave.Instance.ReturnBot(GetComponentInParent<Robert>());
        }
    }
}
