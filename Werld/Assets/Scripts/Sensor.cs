using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int sensorId;
    HashSet<GameObject> sensedObjects;
    
    void Start()
    {
        sensedObjects = new HashSet<GameObject>();
    }

    void Update() {}

    void OnTriggerEnter(Collider collider)
    {
        // ignore the ground 
        if (collider.CompareTag("Ground"))
        {
            return;
        }

        //Debug.Log($"Hit on sensor {sensorId}");
        sensedObjects.Add(collider.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        // ignore the ground 
        if (collider.CompareTag("Ground"))
        {
            return;
        }
        
        sensedObjects.Remove(collider.gameObject);
    }

    public List<GameObject> GetSensedObjects() {
        // TODO: filter out null objects, which can occur if an object
        // enters the collider and is destroyed before exiting
        return sensedObjects.ToList();
    }

    public bool isTriggered() {
        foreach (GameObject obj in sensedObjects)
        {
            if (obj != null){
                return true;
            }
        }
        return false;
    }
}
