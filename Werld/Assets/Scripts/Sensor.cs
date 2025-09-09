using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public List<string> tagWhitelist;
    public GameObject ignoreObject;
    public Vector2 boxOffset;
    public Vector3 boxScale;

    public bool ignoreTriggers = true;

    private float secondsPerScan = 0.1f;
    private float timer;

    List<GameObject> sensedObjects;

    void Start()
    {
        timer = Random.Range(0.0f, secondsPerScan);
        sensedObjects = new List<GameObject>();
    }

    void FixedUpdate()
    {
        if (timer <= 0.0f)
        {
            timer = secondsPerScan;
            MyCollisions();
        }
        timer -= Time.deltaTime;
    }

    void MyCollisions()
    {
        sensedObjects.Clear();
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position + boxOffset.y*transform.right + boxOffset.x*transform.forward, boxScale/2, transform.rotation);
        foreach (Collider collider in hitColliders)
        {
            if (!Ignore(collider))
            {
                //Debug.Log(collider.name);
                sensedObjects.Add(collider.gameObject);
            }
        }
    }

    private bool Ignore(Collider other)
    {
        if (other.isTrigger && ignoreTriggers) return true;
        if (tagWhitelist == null || tagWhitelist.Count == 0)
        {
            return other.CompareTag("Ground") || other.CompareTag("Beacon") || other.CompareTag("SensorIgnore");
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

    public List<GameObject> GetSensedObjects()
    {
        return sensedObjects;
    }

    public bool IsTriggered()
    {
        return sensedObjects.Count != 0;
    }

    // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
    void OnDrawGizmos()
    {
        Gizmos.color = (Application.isPlaying && IsTriggered()) ? Color.red: Color.blue;
        Gizmos.DrawWireCube(gameObject.transform.position + boxOffset.x * transform.forward + boxOffset.y * transform.right, boxScale);
    }
}