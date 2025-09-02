using System.Collections.Generic;
using UnityEngine;

public struct CaveCell
{
    public enum CaveCellType
    {
        Air,
        Wall,
        Border,
        Copper,
        Iron,
        Gold,
        Diamond
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
            case CaveCellType.Copper:
                loot.Add(Items.Lookup("Copper Ore"), 1);
                break;
            case CaveCellType.Iron:
                loot.Add(Items.Lookup("Iron Ore"), 1);
                break;
            case CaveCellType.Gold:
                loot.Add(Items.Lookup("Gold Ore"), 1);
                break;
            case CaveCellType.Diamond:
                loot.Add(Items.Lookup("Diamond"), 1);
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
            case CaveCellType.Copper:
                return 2.5f;
            case CaveCellType.Iron:
                return 5.0f;
            case CaveCellType.Gold:
                return 2.0f;
            case CaveCellType.Diamond:
                return 10.0f;
            default:
                return -1.0f;
        }
    }
}