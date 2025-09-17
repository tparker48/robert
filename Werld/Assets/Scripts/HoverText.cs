using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverText : MonoBehaviour
{
    private Dictionary<string, TextMeshProUGUI> hoverTexts = new Dictionary<string, TextMeshProUGUI>();
    public TextMeshProUGUI textPrefab;

    private int uniqueKeyCounter = 0;

    public float baseFontSize = 18.0f;

    public static HoverText Instance;
    public Transform overlayObject;

    public bool modKeyDown = false;
    public bool toggleKey = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        modKeyDown = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if (Input.GetKeyDown(KeyCode.T))
        {
            toggleKey = !toggleKey;
        }
    }

    public void OverlayText(string textKey, string txt, Vector3 worldPosition, float offset = 0.0f, float fontSizeMult = 1.0f)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition + new Vector3(0, offset, 0));
        float dist = (worldPosition - Camera.main.transform.position).magnitude;
        dist -= 12 * CameraController.Instance.zoomAmount;

        if (!hoverTexts.ContainsKey(textKey))
        {
            hoverTexts[textKey] = Instantiate(textPrefab, overlayObject);
        }
        hoverTexts[textKey].transform.position = screenPosition;
        hoverTexts[textKey].text = txt;
        hoverTexts[textKey].fontSize = baseFontSize * fontSizeMult * (10f / dist);
        hoverTexts[textKey].enabled = Ship.GetFloor(worldPosition) == CameraController.Instance.floor;
        hoverTexts[textKey].enabled = hoverTexts[textKey].enabled && (modKeyDown || toggleKey);

    }

    public void RemoveOverlay(string textKey)
    {
        if (textKey == null)
        {
            return;
        }
        if (hoverTexts.ContainsKey(textKey) && hoverTexts[textKey] != null)
        {
            Destroy(hoverTexts[textKey]);
            hoverTexts.Remove(textKey);
        }
    }

    public void SetOverlayTextColor(string textKey, Color col)
    {
        if (hoverTexts.ContainsKey(textKey))
        {
            hoverTexts[textKey].color = col;
        }
    }

    public string GetUniqueTextKey()
    {
        return "uniqueKey_" + uniqueKeyCounter++;
    }

}
