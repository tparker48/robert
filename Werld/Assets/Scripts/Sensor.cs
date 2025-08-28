using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public int sensorId;
    public List<string> tagWhitelist;
    HashSet<GameObject> sensedObjects;

    void Start()
    {
        sensedObjects = new HashSet<GameObject>();
    }

    void Update() { }

    void OnTriggerEnter(Collider collider)
    {
        // ignore the ground 
        if (ShouldIgnoreTag(collider))
        {
            return;
        }
        sensedObjects.Add(collider.gameObject);
    }
    void OnTriggerExit(Collider collider)
    {
        if (ShouldIgnoreTag(collider))
        {
            return;
        }
        sensedObjects.Remove(collider.gameObject);
    }

    public List<GameObject> GetSensedObjects()
    {
        return sensedObjects.ToList();
    }

    public bool isTriggered()
    {
        foreach (GameObject obj in sensedObjects)
        {
            if (obj != null)
            {
                return true;
            }
        }
        return false;
    }

    private bool ShouldIgnoreTag(Collider other)
    {
        if (tagWhitelist == null || tagWhitelist.Count == 0)
        {
            return other.CompareTag("Ground") || other.CompareTag("Beacon");
        }
        else
        {
            foreach (string tag in tagWhitelist)
            {
                if (other.CompareTag(tag))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
