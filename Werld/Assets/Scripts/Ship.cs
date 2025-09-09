using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public uint bits;

    public List<Floor> floors;
    public Floor floorPrefab;
    public Robert robertPrefab;
    public static float floorHeight = 20;
    public static int maxFloors = 9;
    public List<int> floorCosts;

    public static Ship Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("HI");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        floors = new List<Floor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (floors.Count < 1)
        {
            AddFloor();
        }
    }

    public bool BuyRobert(RobertType typeToBuy)
    {
        if (typeToBuy.initialCost <= bits)
        {
            Robert newRob = Instantiate(robertPrefab);
            newRob.InitType(typeToBuy);
            newRob.transform.position = new Vector3(0, 0.5f, 0);
            bits -= typeToBuy.initialCost;
            return true;
        }
        return false;
    }

    public bool AddFloor()
    {
        int cost = GetNewFloorCost();
        if (cost < 0)
        {
            return false;
        }
        else
        {
            bits -= (uint)cost;
            Floor newFloor = Instantiate(floorPrefab, transform);
            newFloor.transform.position = new Vector3(transform.position.x, transform.position.y + floorHeight * floors.Count, transform.position.z);
            newFloor.SetFloorLevel(floors.Count);
            floors.Add(newFloor);
            return true;
        }
    }

    public int GetNewFloorCost()
    {
        int nextFloorIdx = floors.Count;
        if (nextFloorIdx < maxFloors)
        {
            return (int)floorCosts[nextFloorIdx];
        }
        else
        {
            return -1;
        }
        
    }

    public static int GetFloor(Vector3 position)
    {
        int floor = 1 + (int)(position.y / 20.0f);
        if (floor <= 0)
        {
            floor = -1;
        }
        return floor;
    }
}
