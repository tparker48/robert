using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RobertBeaconInterface : MonoBehaviour
{
    public bool debugListBeacons = false;

    private float cleanTimerPeroid = 10.0f;
    private float cleanTimer;

    HashSet<Beacon> beaconsInRange;

    public Beacon beaconPrefab;

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

    public Response HandleScanBeacons(string rawCmd)
    {
        ScanBeacons scanBeacons = CommandParser.Parse<ScanBeacons>(rawCmd);
        List<Beacon> beacons = GetBeacons();
        ScanBeaconsResponse beaconResponse = new ScanBeaconsResponse();
        beaconResponse.beacons = new Dictionary<string, float[]>();

        for (int i = 0; i < beacons.Count; i++)
        {
            Vector2 position = GetBeaconPosition(beacons[i], scanBeacons.relative);
            beaconResponse.beacons[beacons[i].beaconName] = new float[2] { position.x, position.y };
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

    private bool BeaconInRange(string name, ref Beacon match)
    {
        foreach (Beacon beacon in beaconsInRange)
        {
            if (beacon != null && !beacon.IsDestroyed() && beacon.beaconName == name)
            {
                match = beacon;
                return true;
            }
        }
        match = null;
        return false;
    }

    public void HandleCreateBeacon(string rawCmd)
    {
        CreateBeacon cmd = CommandParser.Parse<CreateBeacon>(rawCmd);
        string name = cmd.beacon_name;

        Beacon match = null;
        if (!BeaconInRange(name, ref match))
        {
            Beacon beacon = Instantiate(beaconPrefab);
            beacon.SetName(name);
            beacon.SetColor(Color.magenta);
        }
    }

    public void HandleDeleteBeacon(string rawCmd)
    {
        CreateBeacon cmd = CommandParser.Parse<CreateBeacon>(rawCmd);
        string name = cmd.beacon_name;

        Beacon match = null;
        if (BeaconInRange(name, ref match))
        {
            Destroy(match);
        }
    }
}
