using UnityEngine;
using UnityEngine.UIElements;

public class Beacon : MonoBehaviour
{
    public string beaconName;

    public Vector2 GetRelativePosition(Transform source)
    {
        float dx = transform.position.x - source.position.x;
        float dy = transform.position.z - source.position.z;
        float fx = source.forward.x;
        float fy = source.forward.z;
        float ry = source.right.z;
        float c = fy / ry;
        float af = (dx - dy * c) / (fx - fy * c);
        float ar = (dy - af * fy) / ry;
        return new Vector2(af,ar);
    }
}
