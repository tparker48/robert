using UnityEngine;

public class Beacon : MonoBehaviour
{
    public string beaconName;

    public void Start()
    {
        Label beaconLabel = GetComponent<Label>();
        beaconLabel.text = "[" + beaconName + "]";
        beaconLabel.textColor = Color.cyan;
    }

    public void SetName(string name)
    {
        Label beaconLabel = GetComponent<Label>();
        beaconLabel.text = "[" + name + "]";
        beaconName = name;
    }

    public void SetColor(Color color)
    {
        Label beaconLabel = GetComponent<Label>();
        beaconLabel.textColor = color;
    }

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
        return new Vector2(af, ar);
    }
}
