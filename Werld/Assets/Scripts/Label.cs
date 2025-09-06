using UnityEngine;

public class Label : MonoBehaviour
{
    public string text = "label";
    public Color textColor;
    public float offset = 1.0f;

    private string key;

    // Start is called before the first frame update
    void Start()
    {
        key = HoverText.Instance.GetUniqueTextKey();
        HoverText.Instance.OverlayText(key, text, transform.position, offset);

    }

    // Update is called once per frame
    void Update()
    {
        HoverText.Instance.OverlayText(key, text, transform.position, offset);
        HoverText.Instance.SetOverlayTextColor(key, textColor);
    }

    public void OnDestroy()
    {
        HoverText.Instance.RemoveOverlay(key);
    }
}
