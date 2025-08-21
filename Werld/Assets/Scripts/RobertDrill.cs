using System;
using UnityEngine;

public class RobertDrill : MonoBehaviour
{
    private bool mining = false;
    private float mineSpeed = 1.0f;
    private float mineTimer = 0.0f;
    private Vector3 mineTarget;
    private RobertInventory inventory;

    void Start()
    {

    }

    void Update()
    {
        if (mining)
        {
            if (mineTimer <= 0.0f)
            {
                inventory.AddFrom(Cave.Instance.MineCell(mineTarget));
                mining = false;
            }
            mineTimer -= Time.deltaTime;
        }
    }

    public void Halt()
    {
        mining = false;
    }

    public void LinkInventory(ref RobertInventory inventory)
    {
        this.inventory = inventory;
    }

    public void HandleMineCommand(MineCommand mineCommand)
    {
        if (mining) return;

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
            mineTimer = Cave.Instance.GetCellToughness(mineTarget) * mineSpeed;
            mining = true;
        }
    }

    public bool Busy()
    {
        return mining;
    }
}
