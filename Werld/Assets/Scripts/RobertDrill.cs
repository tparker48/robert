using System;
using UnityEngine;

public class RobertDrill : RobertTimedTaskExecutor<MineCommand>
{
    private float mineSpeed = 1.0f;
    private int scanRadius = 2;

    private Vector3 mineTarget;
    private RobertInventory inventory;

    void Start()
    {
        inventory = GetComponentInParent<RobertInventory>();
    }

    void Update()
    {
        UpdateTask(Time.deltaTime);
    }

    protected override void ExecuteOnTaskEnd(MineCommand task)
    {
        inventory.AddFrom(Cave.Instance.MineCell(mineTarget));
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

    public int[,] HandleMineralQuery(MineralQuery _)
    {
        return Cave.Instance.Scan(transform.position, scanRadius); 
    }

    public bool InMine()
    {
        return true;
    }
}
