using System.Collections.Generic;
using UnityEngine;

public struct CaveCell
{
    public enum CaveCellType
    {
        Air,
        Wall,
        Ore,
        Border
    }

    public CaveCellType type;
    public BoxCollider collider;

    public CaveCell(CaveCellType type)
    {
        this.type = type;
        collider = null;
    }

    public ItemGroup MineCell()
    {
        ItemGroup loot = new ItemGroup();

        switch (type)
        {
            case CaveCellType.Wall:
                loot.Add(Items.Lookup("Stone"), (uint)Random.Range(1, 3));
                break;
            case CaveCellType.Ore:
                loot.Add(Items.Lookup("Stone"), 1);
                break;
        }

        type = CaveCellType.Air;
        return loot;
    }

    public bool CanMine()
    {
        return type != CaveCellType.Air && type != CaveCellType.Border;
    }

    public float GetToughness()
    {
        switch (type)
        {
            case CaveCellType.Wall:
                return 2.0f;
            case CaveCellType.Ore:
                return 2.5f;
            default:
                return -1.0f;
        }
    }
}