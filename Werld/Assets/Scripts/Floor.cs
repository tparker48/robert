using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public List<Room> rooms;

    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Room room in rooms)
        {
            room.AddEquipment(myPrefab);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
