
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using SimplexNoise;
using Unity.VisualScripting;
using UnityEngine;

public class Cave : MonoBehaviour
{
    public int numCASteps;
    public int caSustainThreshold;
    public int caSpawnThreshold;
    public float initialWallOdds;

    public float copperNoiseScale;
    public float copperNoiseThresh;
    public float ironNoiseScale;
    public float ironNoiseThresh;
    public float goldNoiseScale;
    public float goldNoiseThresh;
    public float diamondNoiseScale;
    public float diamondNoiseThresh;
    private int noiseOffset = 1704;

    public bool regen = true;

    public Dictionary<Robert, Vector3> roberts;


    private CaveCell[,] cells;
    private Vector3 cellSize;
    private Vector3 spawnPoint;
    public int caveSize = 120;


    public static Cave Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cellSize = new Vector3(1, 3, 1);
        roberts = new Dictionary<Robert, Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (regen)
        {
            regen = false;
            Regenerate();
        }

        RefreshColliders();
    }

    public void Regenerate()
    {
        cells = new CaveCell[caveSize, caveSize];
        BuildCave();
        PlaceOre();
        SetSpawnPoint();
        foreach (Robert bot in roberts.Keys)
        {
            bot.transform.position = spawnPoint;
        }
    }

    public void AddBot(Robert bot)
    {
        roberts[bot] = bot.transform.position;
        bot.transform.position = spawnPoint;
        bot.GetComponent<RobertMineInterface>().SetMineState(true);
    }

    public void ReturnBot(Robert bot)
    {
        bot.transform.position = roberts[bot];
        roberts.Remove(bot);
        bot.GetComponent<RobertMineInterface>().SetMineState(false);
    }

    private void SetSpawnPoint()
    {
        for (int x = 0; x < caveSize; x++)
        {
            for (int y = 0; y < caveSize; y++)
            {
                if (cells[x, y].type == CaveCell.CaveCellType.Air)
                {
                    spawnPoint = new Vector3(x, transform.position.y + 0.5f, y);
                    return;
                }
            }
        }
        
    }

    private void BuildCave()
    {
        bool[,] initialState = GenerateCaveState();

        for (int x = 0; x < caveSize; x++)
        {
            for (int y = 0; y < caveSize; y++)
            {
                if (x == 0 || x == caveSize - 1 || y == 0 || y == caveSize - 1)
                {
                    cells[x, y] = new CaveCell(CaveCell.CaveCellType.Border);
                }
                else if (initialState[x, y])
                {
                    cells[x, y] = new CaveCell(CaveCell.CaveCellType.Air);
                }
                else
                {
                    cells[x, y] = new CaveCell(CaveCell.CaveCellType.Wall);
                }
            }
        }
    }

    private bool[,] GenerateCaveState()
    {
        bool[,,] state = new bool[2, caveSize, caveSize];
        int newState = 1;
        int oldState = 0;
        int swap;

        // init with noise
        for (int x = 0; x < caveSize; x++)
        {
            for (int y = 0; y < caveSize; y++)
            {
                state[oldState, x, y] = UnityEngine.Random.Range(0.0f, 1.0f) > initialWallOdds;

                if (x == 0 || y == 0 || x == caveSize - 1 || y == caveSize - 1)
                {
                    state[oldState, x, y] = false;
                }
            }
        }

        // run CA rules
        for (int step = 0; step < numCASteps; step++)
        {
            for (int x = 0; x < caveSize; x++)
            {
                for (int y = 0; y < caveSize; y++)
                {
                    state[newState, x, y] = RunCaveCA(ref state, oldState, x, y);
                }
            }

            // flip states
            swap = oldState;
            oldState = newState;
            newState = swap;
        }

        // return final state
        bool[,] finalState = new bool[caveSize, caveSize];
        for (int x = 0; x < caveSize; x++)
        {
            for (int y = 0; y < caveSize; y++)
            {
                finalState[x, y] = state[oldState, x, y];
            }
        }
        return finalState;
    }

    private bool RunCaveCA(ref bool[,,] state, int oldState, int x, int y)
    {
        int neighborCount = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                // don't count self
                if (i == x && j == y) continue;

                // don't look out of bounds
                if (i >= caveSize || i < 0 || j >= caveSize || j < 0) continue;

                neighborCount += state[oldState, i, j] ? 1 : 0;
            }
        }

        if (state[oldState, x, y]) return neighborCount >= caSustainThreshold;
        else return neighborCount >= caSpawnThreshold;
    }

    private void PlaceOre()
    {
        Noise.Seed = UnityEngine.Random.Range(0, 10000000);
        for (int x = 0; x < caveSize; x++)
        {
            for (int y = 0; y < caveSize; y++)
            {
                // todo swap with perlin noise
                if (cells[x, y].type == CaveCell.CaveCellType.Wall)
                {
                    if (Noise.CalcPixel2D(x, y, copperNoiseScale) >= copperNoiseThresh)
                    {
                        cells[x, y].type = CaveCell.CaveCellType.Copper;
                    }
                    if (Noise.CalcPixel2D(x +noiseOffset, y+ noiseOffset, ironNoiseScale) >= ironNoiseThresh)
                    {
                        cells[x, y].type = CaveCell.CaveCellType.Iron;
                    }
                    if (Noise.CalcPixel2D(x + noiseOffset*2, y + noiseOffset*2, goldNoiseScale) >= goldNoiseThresh)
                    {
                        cells[x, y].type = CaveCell.CaveCellType.Gold;
                    }
                    if (Noise.CalcPixel2D(x + noiseOffset*3, y + noiseOffset*3, diamondNoiseScale) >= diamondNoiseThresh)
                    {
                        cells[x, y].type = CaveCell.CaveCellType.Diamond;
                    }
                }
            }
        }
    }

    void RefreshColliders()
    {
        HashSet<Vector2Int> addList = new HashSet<Vector2Int>();

        foreach (Robert robert in roberts.Keys)
        {
            int robX = Mathf.RoundToInt(robert.transform.position.x);
            int robY = Mathf.RoundToInt(robert.transform.position.z);

            for (int xOffset = -10; xOffset <= 10; xOffset++)
            {
                for (int yOffset = -10; yOffset <= 10; yOffset++)
                {
                    int x = Math.Clamp(robX + xOffset, 0, caveSize - 1);
                    int y = Math.Clamp(robY + yOffset, 0, caveSize - 1);

                    if (CellNeedsCollider(robX, robY, x, y))
                    {
                        addList.Add(new Vector2Int(x, y));
                    }
                    else
                    {
                        if (cells[x, y].collider != null)
                        {
                            Destroy(cells[x, y].collider);
                            cells[x, y].collider = null;
                        }
                    }
                }
            }
        }

        foreach (Vector2Int colliderCoords in addList)
        {
            int x = colliderCoords.x;
            int y = colliderCoords.y;
            if (cells[x, y].collider == null)
            {
                cells[x, y].collider = this.AddComponent<BoxCollider>();
                cells[x, y].collider.enabled = true;
                cells[x, y].collider.center = new Vector3(x, cellSize.y / 2, y);
                cells[x, y].collider.size = cellSize;
            }
        }
    }

    private bool CellNeedsCollider(int robX, int robY, int x, int y)
    {
        float colliderRadius = 2.5f;

        // Non-collision cells
        if (cells[x, y].type == CaveCell.CaveCellType.Air) return false;

        // Too far away
        Vector2 robPosition = new Vector2(robX, robY);
        Vector2 colliderPosition = new Vector2(transform.position.x + x, transform.position.z + y);
        float distance = Vector2.Distance(robPosition, colliderPosition);
        if (distance > colliderRadius) return false;

        return true;
    }

    public Vector2Int GetCellCoords(Vector3 worldCoords)
    {
        int x = (int)MathF.Round(worldCoords.x - transform.position.x);
        int y = (int)MathF.Round(worldCoords.z - transform.position.z);
        return new Vector2Int(x, y);
    }

    public bool CellInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < caveSize && y < caveSize;
    }
    
    public float GetCellToughness(Vector3 worldCoords)
    {
        Vector2Int coords = GetCellCoords(worldCoords);
        if (CanMine(coords.x, coords.y))
        {
            return cells[coords.x, coords.y].GetToughness();
        }
        return 0.0f;
    }

    public ItemGroup MineCell(Vector3 worldCoords)
    {
        Vector2Int coords = GetCellCoords(worldCoords);
        int x = coords.x;
        int y = coords.y;

        if (CanMine(x,y))
        {
            if (cells[x, y].collider != null)
            {
                Destroy(cells[x, y].collider);
                cells[x, y].collider = null;
            }
            return cells[x, y].MineCell();
        }
        return new ItemGroup();
    }

    public bool CanMine(int x, int y)
    {
        return CellInBounds(x, y) && cells[x, y].CanMine();
    }

    public bool CanMine(Vector3 worldCoords)
    {
        Vector2Int coords = GetCellCoords(worldCoords);
        return CellInBounds(coords.x, coords.y) && cells[coords.x, coords.y].CanMine();
    }

    public string Scan(Vector3 worldCoords, int scanRadius)
    {
        Vector2Int coords = GetCellCoords(worldCoords);
        string scanResult = "";

        for (int yOffset = -scanRadius; yOffset <= scanRadius; yOffset++)
        {
            for (int xOffset = -scanRadius; xOffset <= scanRadius; xOffset++)
            {
                int x = coords.x + xOffset;
                int y = coords.y - yOffset;

                if (xOffset == 0 && yOffset == 0)
                {
                    scanResult += -1; // represents robert
                }
                else if (CellInBounds(x, y))
                {
                    scanResult += (int)cells[x, y].type;
                }
                else
                {
                    scanResult += (int)CaveCell.CaveCellType.Air;
                }
                
                scanResult += ' ';
            }
            scanResult += '\n';
        }
        
        return scanResult;
    }

    void OnDrawGizmos()
    {
        if (cells != null)
        {
            for (int x = 0; x < caveSize; x++)
            {
                for (int y = 0; y < caveSize; y++)
                {
                    if (cells[x, y].type != CaveCell.CaveCellType.Air)
                    {
                        if (cells[x, y].type == CaveCell.CaveCellType.Border)
                        {
                            Gizmos.color = Color.white;
                        }
                        else if (cells[x, y].type == CaveCell.CaveCellType.Wall)
                        {
                            Gizmos.color = Color.grey;
                        }
                        else if (cells[x, y].type == CaveCell.CaveCellType.Copper)
                        {
                            Gizmos.color = Color.red;
                        }
                        else if (cells[x, y].type == CaveCell.CaveCellType.Iron)
                        {
                            Gizmos.color = Color.white;
                        }
                        else if (cells[x, y].type == CaveCell.CaveCellType.Gold)
                        {
                            Gizmos.color = Color.yellow;
                        }
                        else if (cells[x, y].type == CaveCell.CaveCellType.Diamond)
                        {
                            Gizmos.color = Color.cyan;
                        }
                        

                        Gizmos.DrawCube(transform.position + new Vector3(x, cellSize.y / 2, y), cellSize);
                    }
                }
            }
        }
    }


}
