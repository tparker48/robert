using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuilder : MonoBehaviour
{
    public Button buildButton;
    public bool buildModeActive = false;
    public TextMeshProUGUI equipmentSelectionText;

    public Button demolishButton;
    public bool demoModeActive = false;
    public TextMeshProUGUI demoBitsText;

    public Button NewFloorButton;
    public TextMeshProUGUI floorCostText;


    public static ShipBuilder Instance = null;

    public Room selectedRoom = null;

    public List<GameObject> equipmentPrefabs;
    private int equipmentSelectedIndex = 0;

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
        equipmentSelectionText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buildModeActive)
        {
            CheckMouseOverRoom();

            if (Input.mouseScrollDelta.y > 0)
            {
                equipmentSelectedIndex++;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                equipmentSelectedIndex--;
            }
            equipmentSelectedIndex = Math.Clamp(equipmentSelectedIndex, 0, equipmentPrefabs.Count - 1);
            
            equipmentSelectionText.text = equipmentPrefabs[equipmentSelectedIndex].name;

            if (Input.GetMouseButtonDown(0) && selectedRoom != null)
            {
                if (selectedRoom.CanBuild())
                {
                    selectedRoom.AddEquipment(equipmentPrefabs[equipmentSelectedIndex]);
                }
            }
        }

        if (demoModeActive)
        {
            CheckMouseOverRoom();
            if (selectedRoom != null)
            {
                demoBitsText.text = $"+{selectedRoom.GetDemolishValue()} bits";
            }
            else
            {
                demoBitsText.text = "";
            }

            if (Input.GetMouseButtonDown(0) && selectedRoom != null)
            {
                selectedRoom.Demolish();
            }
        }
    }

    private void CheckMouseOverRoom()
    {
        selectedRoom = null;
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 50.0f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Build Selector"))
            {
                selectedRoom = hit.transform.GetComponentInParent<Room>();
            }
        }
    }

    public void ToggleDemoMode()
    {
        if (buildModeActive) { return; }

        demoModeActive = !demoModeActive;
        if (demoModeActive)
        {
            demolishButton.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
            demoBitsText.text = "";
            demoBitsText.enabled = true;
        }
        else
        {
            demolishButton.GetComponentInChildren<TextMeshProUGUI>().text = "DEMOLISH";
            demoBitsText.enabled = false;
        }
        foreach (Floor floor in Ship.Instance.floors)
        {
            foreach (Room room in floor.rooms)
            {
                room.SetBuildSelectorActive(demoModeActive);
            }
        }
    }

    public void ToggleBuildMode()
    {
        if (demoModeActive) { return; }

        buildModeActive = !buildModeActive;
        
        if (buildModeActive)
        {
            buildButton.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
            equipmentSelectionText.enabled = true;
        }
        else
        {
            buildButton.GetComponentInChildren<TextMeshProUGUI>().text = "BUILD";
            equipmentSelectionText.enabled = false;
        }

        foreach (Floor floor in Ship.Instance.floors)
        {
            foreach (Room room in floor.rooms)
            {
                room.SetBuildSelectorActive(buildModeActive);
            }
        }
    }
 }
