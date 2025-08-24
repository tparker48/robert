using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobertBeaconScanner : MonoBehaviour
{
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
            Debug.Log("BEACON GAINED");
            beaconsInRange.Add(other.GetComponent<Beacon>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Beacon"))
        {
            Debug.Log("BEACON LOST");
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
        //beaconsInRange.RemoveWhere(beacon => beacon==null || beacon.IsDestroyed());
    }

    public Response HandleBeaconQuery(BeaconQuery beaconQuery)
    {
        List<Beacon> beacons = GetBeacons();
        BeaconQueryResponse beaconResponse = new BeaconQueryResponse();
        beaconResponse.beacons = new string[beacons.Count];
        beaconResponse.positions = new float[beacons.Count, 2];

        for (int i = 0; i < beacons.Count; i++)
        {
            beaconResponse.beacons[i] = beacons[i].beaconName;
            Vector2 position = GetBeaconPosition(beacons[i], beaconQuery.relative);
            beaconResponse.positions[i, 0] = position.x;
            beaconResponse.positions[i, 1] = position.y;
        }
        Debug.Log("Beacon Query Complete");
        return beaconResponse;
    }

    Vector2 GetBeaconPosition(Beacon beacon, bool relative)
    {
        if (relative)
        {
            return beacon.GetRelativePosition(transform);
        }
        else
        {
            return new Vector2(beacon.transform.position.x, beacon.transform.position.z);
        }
    }
}
