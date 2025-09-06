using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public uint bits;

    public List<Floor> floors;
    public Floor floorPrefab;
    public static float floorHeight = 20;

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
        if (floors.Count < 3)
        {
            AddFloor();
        }
    }

    public void AddFloor()
    {
        Floor newFloor = Instantiate(floorPrefab, transform);
        newFloor.transform.position = new Vector3(transform.position.x, transform.position.y + floorHeight * floors.Count, transform.position.z);
        newFloor.SetFloorLevel(floors.Count); // 0 1 2 3 ...
        floors.Add(newFloor);
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

    // public BuildBotResponse HandleBuildBotCommand(BuildBotCommand cmd) 

    public ShipBitsQueryResponse HandleGetBitsQuery(ShipBitsQuery query)
    {
        ShipBitsQueryResponse response = new ShipBitsQueryResponse();
        response.bits = bits;
        return response;
    }
    
}
