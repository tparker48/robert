using UnityEngine;


public class Room : MonoBehaviour
{
    public GameObject doorObject;
    public Light lightObject;
    public GameObject structure;
    public Sensor roomSensor;
    public Sensor doorSensor;

    public bool open = false;
    public bool lightsOn = false;
    public bool locked = true;

    private GameObject equipment = null;


    void Start()
    {
        GetComponentInChildren<Beacon>().beaconName = name;
    }

    void Update()
    {
        open = open && !locked;
        doorObject.SetActive(!open);
        lightObject.enabled = lightsOn;
    }

    public int GetDemolishValue()
    {
        return 1;//equipment.demoValue;
    }

    public void Demolish()
    {
        Ship.Instance.bits += (uint)GetDemolishValue();
        Destroy(equipment);
        equipment = null;
    }

    public void AddEquipment(GameObject prefab)
    {
        equipment = Instantiate(prefab, transform);
    }

    public bool HasObject(GameObject match)
    {
        foreach (GameObject obj in roomSensor.GetSensedObjects())
        {
            if (obj == match)
            {
                return true;
            }
        }
        foreach (GameObject obj in doorSensor.GetSensedObjects())
        {
            if (obj == match)
            {
                return true;
            }
        }
        return false;
    }

    public bool CanBuild()
    {
        Debug.Log($"Checking if can build in room: {name}");
        return equipment == null;
    }
}
