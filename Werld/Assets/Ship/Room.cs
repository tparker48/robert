using UnityEngine;


public class Room : MonoBehaviour
{
    public GameObject doorObject;
    public Light lightObject;
    public GameObject structure;

    public bool open = false;
    public bool lightsOn = false;
    public bool locked = true;
    public bool highlighted = false;

    public Material notSelected, selectedGood, selectedBad;

    public GameObject highlighter;


    void Start()
    {
        GetComponentInChildren<Beacon>().beaconName = this.name;
    }

    void Update()
    {
        open = open && !locked;
        doorObject.SetActive(!open);
        lightObject.enabled = lightsOn;

        if (highlighted)
        {
            CheckSelected();
        }
    }

    public void SetBuildSelectorActive(bool active) {
        highlighter.GetComponent<MeshRenderer>().enabled = active;
        highlighted = true;
    }

    public void CheckSelected()
    {
        if (ShipBuilder.Instance.selectedRoom == this)
        {
            highlighter.GetComponent<Renderer>().material = selectedGood;
        }
        else
        {
            highlighter.GetComponent<Renderer>().material = notSelected;
        }
        
    }
}
