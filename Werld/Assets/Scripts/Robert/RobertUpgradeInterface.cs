using UnityEngine;

public class RobertUpgradeInterface : MonoBehaviour
{
    private RobertTraits traits;

    public void Start()
    {
        traits = GetComponent<RobertTraits>();
    }

    public void HandleUpgrade(string rawCmd)
    {
        traits.Upgrade();
    }

    public void HandleGetUpgradeRequirements()
    {
        
    }
}
