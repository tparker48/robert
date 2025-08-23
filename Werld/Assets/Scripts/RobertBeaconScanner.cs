using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobertBeaconScanner : MonoBehaviour
{
    public BoxCollider beaconCollider;

    public bool debugListBeacons = false;

    private float cleanTimerPeroid = 10.0f;
    private float cleanTimer;

    HashSet<Beacon> beaconsInRange;

    // Start is called before the first frame update
    void Start()
    {
        beaconsInRange = new HashSet<Beacon>();
        cleanTimer = cleanTimerPeroid;
    }

    // Update is called once per frame
    void Update()
    {
        cleanTimer -= Time.deltaTime;
        if (cleanTimer <= 0.0f)
        {
            CleanBeaconSet();
            cleanTimer = cleanTimerPeroid;
        }


        if (debugListBeacons)
        {
            foreach (Beacon beacon in beaconsInRange)
            {
                Debug.Log(beacon.beaconName);
            }
            debugListBeacons = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beacon"))
        {
            beaconsInRange.Add(other.GetComponent<Beacon>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beacon"))
        {
            beaconsInRange.Remove(other.GetComponent<Beacon>());
        }
    }

    public List<Beacon> GetBeacons()
    {
        List<Beacon> beacons = new List<Beacon>();
        foreach (Beacon beacon in beaconsInRange)
        {
            if (beacon != null && !beacon.IsDestroyed())
            {
                beacons.Add(beacon);
            }
        }
        return beacons;
    }

    private void CleanBeaconSet()
    {
        beaconsInRange.RemoveWhere(beacon => beacon==null || beacon.IsDestroyed());
    }
}
