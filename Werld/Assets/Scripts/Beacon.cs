using UnityEngine;

public class Beacon : MonoBehaviour
{
    public string beaconName;

    public Vector2 GetCoords()
    {
        return new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
    }
}
