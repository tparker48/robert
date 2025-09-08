using UnityEngine;

public class RobertUpgradeInterface : MonoBehaviour
{
    private RobertTraits traits;

    public void Start()
    {
        traits = GetComponent<RobertTraits>();
    }

    public void HandleUpgrade(string _)
    {
        traits.Upgrade();
    }

    public Response HandleGetUpgradeRequirements(string _)
    {
        GetUpgradeCostResponse response = new GetUpgradeCostResponse();
        response.items = traits.GetUpgradeRequirements().ToStringKeys();
        return response;
    }

    public Response HandleGetBotType(string _)
    {
        GetBotTypeResponse response = new GetBotTypeResponse();
        response.type = traits.GetRobertType();
        return response;
    }
}
