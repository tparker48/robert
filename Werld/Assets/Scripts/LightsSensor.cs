using UnityEngine;

public class LightsSensor : MonoBehaviour
{
    private Sensor doorSensor;
    private Room room;

    private bool lightState;
    public float shutoffTime = 1.5f;
    private float lightTimer;


    // Start is called before the first frame update
    void Start()
    {
        doorSensor = GetComponent<Sensor>();
        room = GetComponentInParent<Room>();
        lightState = false;
        lightTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (doorSensor.isTriggered())
        {
            lightTimer = shutoffTime;
            lightState = true;
        }
        else if (lightState)
        {
            lightTimer -= Time.deltaTime;
            if (lightTimer <= 0.0f)
            {
                lightState = false;
            }
        }
        room.lightsOn = lightState;
    }
}
