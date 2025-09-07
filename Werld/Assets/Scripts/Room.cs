using UnityEngine;


public class Room : MonoBehaviour
{
    public GameObject doorObject;
    public Light lightObject;
    public GameObject structure;

    public bool open = false;
    public bool lightsOn = false;
    public bool locked = true;

    public GameObject equipment = null;


    void Start()
    {
        GetComponentInChildren<Beacon>().beaconName = this.name;
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

    public bool CanBuild()
    {
        return equipment == null;
    }
}
