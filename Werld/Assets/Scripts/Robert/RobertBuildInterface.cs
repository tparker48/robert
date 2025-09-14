using System.Collections.Generic;
using UnityEngine;

public class RobertBuildInterface : MonoBehaviour
{
    public void HandleBuildRoomEquipment(string rawCmd)
    {
        Room roomToBuild = null;
        if (GetRoom(ref roomToBuild))
        {
            BuildRoomEquipment cmd = CommandParser.Parse<BuildRoomEquipment>(rawCmd);
            string equipmentName = cmd.equipment;
            foreach (GameObject prefab in Ship.Instance.equipmentPrefabs)
            {
                if (prefab.name == equipmentName)
                {
                    Debug.Log("Trying to build now...");
                    Ship.Instance.BuildEquipment(prefab, ref roomToBuild);
                    return;
                }
            }
        }
    }

    private bool GetRoom(ref Room foundRoom)
    {
        int floor = Ship.GetFloor(transform.position);
        foreach (Room room in Ship.Instance.floors[floor-1].rooms)
        {
            Debug.Log(room.name);
            Debug.Log(gameObject);
            if (room.HasObject(gameObject))
            {
                foundRoom = room;
                return true;
            }
        }
        return false;
    }
}
