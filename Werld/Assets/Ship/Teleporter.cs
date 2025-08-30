using UnityEngine;

public class Teleporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TeleportRobertToMine(Robert robert)
    {
        // regen the cave, if no roberts are in there already
        if (Cave.Instance.roberts.Count == 0)
        {
            Cave.Instance.Regenerate();
            Cave.Instance.AddBot(robert);
        }
    }
}
