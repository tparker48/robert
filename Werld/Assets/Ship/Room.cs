using UnityEngine;


public class Room : MonoBehaviour
{
    public GameObject doorObject;
    public Light lightObject;
    public GameObject structure;

    public bool open = false;
    public bool lightsOn = false;
    public bool locked = true;

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
}
