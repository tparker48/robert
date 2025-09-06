using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Room> rooms;
    public GameObject ControlRoomWall;
    public GameObject ControlRoom;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetFloorLevel(int level)
    {
        if (level == 0)
        {
            ControlRoom.SetActive(true);
            ControlRoomWall.SetActive(false);
        }
        else
        {
            ControlRoom.SetActive(false);
            ControlRoomWall.SetActive(true);
        }
    }
}
