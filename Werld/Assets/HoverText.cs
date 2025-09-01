using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour
{
    private Dictionary<string, TextMeshProUGUI> hoverTexts = new Dictionary<string, TextMeshProUGUI>();
    public TextMeshProUGUI textPrefab;

    public float baseFontSize = 24.0f;

    public static HoverText Instance;

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
    }

    public void OverlayText(string textKey, string txt, Vector3 worldPosition, float offset = 0.0f)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition + new Vector3(0, offset, 0));
        float dist = (worldPosition - Camera.main.transform.position).magnitude;

        Debug.Log(screenPosition);
        if (!hoverTexts.ContainsKey(textKey))
        {
            hoverTexts[textKey] = Instantiate(textPrefab, transform);
        }
        hoverTexts[textKey].transform.position = screenPosition;
        hoverTexts[textKey].text = txt;
        hoverTexts[textKey].fontSize = baseFontSize * (10f / dist);
        hoverTexts[textKey].enabled = Ship.GetFloor(worldPosition) == CameraController.Instance.floor;
        
    }

    public void RemoveOverlay(string textKey)
    {
        if (hoverTexts.ContainsKey(textKey))
        {
            DestroyImmediate(hoverTexts[textKey]);
            hoverTexts.Remove(textKey);
        }
    }
}
