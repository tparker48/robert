using System.IO;
using UnityEngine;

public class RobertDrill : RobertTimedTaskExecutor<MineCommand>
{
    private float mineSpeed = 1.0f;
    public int scanRadius = 35;
    private bool isInMine = false;

    private Vector3 mineTarget;
    private ItemContainer inventory;

    void Start()
    {
        inventory = GetComponentInParent<ItemContainer>();
    }

    void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    protected override void ExecuteOnTaskEnd(MineCommand task)
    {
        inventory.AddItems(Cave.Instance.MineCell(mineTarget));
    }

    public void HandleMineCommand(MineCommand mineCommand)
    {
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

    public string HandleScanMine(ScanMine _)
    {
        string filepath = Application.dataPath + "/tempdata/mine_scan.txt";
        string map = Cave.Instance.Scan(transform.position, scanRadius);
        File.WriteAllText(filepath, map);
        return filepath;
    }

    public bool IsInMine()
    {
        return isInMine;
    }

    public void SetMineState(bool inMine)
    {
        isInMine = inMine;
    }
}
