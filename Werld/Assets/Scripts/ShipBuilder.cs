using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuilder : MonoBehaviour
{
    public Button buildButton;
    public bool buildModeActive = false;

    public TextMeshProUGUI equipmentToBuild;

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
        equipmentToBuild.enabled = false;
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

            equipmentToBuild.text = equipmentPrefabs[equipmentSelectedIndex].name;
        }

        if (Input.GetMouseButtonDown(0) && selectedRoom != null)
        {
            if (selectedRoom.CanBuild())
            {
                Debug.Log("Hi I can build now!");
                selectedRoom.AddEquipment(equipmentPrefabs[equipmentSelectedIndex]);
            }
            else
            {
                Debug.Log("I CANNOT BUILD NOW");
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

    public void ToggleBuildMode()
    {
        buildModeActive = !buildModeActive;
        if (buildModeActive)
        {
            buildButton.GetComponentInChildren<TextMeshProUGUI>().text = "CANCEL";
            equipmentToBuild.enabled = true;

            foreach (Floor floor in Ship.Instance.floors)
            {
                foreach (Room room in floor.rooms)
                {
                    Debug.Log("Room");
                    room.SetBuildSelectorActive(true);
                }
            }

        }
        else
        {
            buildButton.GetComponentInChildren<TextMeshProUGUI>().text = "BUILD";
            equipmentToBuild.enabled = false;

            foreach (Floor floor in Ship.Instance.floors)
            {
                foreach (Room room in floor.rooms)
                {
                    room.SetBuildSelectorActive(false);
                }
            }
        }
    }
 }
