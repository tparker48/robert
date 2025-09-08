using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobertSensorInterface : MonoBehaviour
{
    public Sensor left, middle, right;

    void Start()
    {
        if (left == null || middle == null || right == null)
        {
            Debug.Log("ERROR: 'Left', 'Middle' and 'Right' sensors must be linked to RobertSensorInterface objects.");
        }
    }
    void Update()
    {
    }

    public bool[] CheckSensors()
    {
        return new bool[3] { left.isTriggered(), middle.isTriggered(), right.isTriggered() };
    }

    public bool GetObjectOfType<T>(ref T result)
    {
        foreach (GameObject obj in middle.GetSensedObjects())
        {
            T comp = obj.GetComponent<T>();
            if (comp != null)
            {
                result = comp;
                return true;
            }

            T compInChildren = obj.GetComponentInChildren<T>();
            if (compInChildren != null)
            {
                result = compInChildren;
                return true;
            }

            T compInParent = obj.GetComponentInParent<T>();
            if (compInParent != null)
            {
                result = compInParent;
                return true;
            }
        }
        return false;
    }

    public List<GameObject> GetSensedObjects()
    {
        List<GameObject> allSensedObjects = left.GetSensedObjects();
        allSensedObjects.AddRange(middle.GetSensedObjects());
        allSensedObjects.AddRange(right.GetSensedObjects());
        return allSensedObjects;
    }

    public Response HandleCheckSensors(string _)
    {
        CheckSensorsResponse sensor_response = new CheckSensorsResponse();
        sensor_response.readings = CheckSensors();
        return sensor_response;
    }
}
