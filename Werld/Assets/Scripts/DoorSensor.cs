using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    private Sensor doorSensor;
    private Room room;

    private bool openState;
    public float openTime = 1.5f;
    private float openTimer;


    // Start is called before the first frame update
    void Start()
    {
        doorSensor = GetComponent<Sensor>();
        room = GetComponentInParent<Room>();
        openState = false;
        openTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorSensor.IsTriggered())
        {
            openTimer = openTime;
            openState = true;
        }
        else if (openState)
        {
            openTimer -= Time.deltaTime;
            if (openTimer <= 0.0f)
            {
                openState = false;
            }
        }
        room.open = openState;
    }
}
