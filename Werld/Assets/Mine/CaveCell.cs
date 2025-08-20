using UnityEngine;

public struct CaveCell
{
    public enum CaveCellType
    {
        Air,
        Wall,
        Border,
        Ore
    }

    public CaveCellType type;
    public BoxCollider collider;

    public CaveCell(CaveCellType type)
    {
        this.type = type;
        collider = null;
    }
}