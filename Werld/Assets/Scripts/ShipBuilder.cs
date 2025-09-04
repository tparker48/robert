using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuilder : MonoBehaviour
{
    public Button buildButton;
    public bool buildModeActive = false;

    public static ShipBuilder Instance = null;

    public Room selectedRoom = null;

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

    }

    // Update is called once per frame
    void Update()
    {
        if (buildModeActive)
        {
            CheckMouseOverRoom();            
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
