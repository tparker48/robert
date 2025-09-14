using System.Collections.Generic;
using UnityEngine;

public class RobertTraits : MonoBehaviour
{
    private Dictionary<string, float> traits;
    private RobertType botType = null;
    private int level = 0;

    private ItemContainer inventory;

    void Awake()
    {
        traits = new Dictionary<string, float>
            {
                {"moveSpeed", 0.0f},
                {"capacity", 0.0f},
                {"mineSpeed", 0.0f},
                {"harvestMultiplier", 0.0f},
                {"sellValueMultiplier", 0.0f},
            };
        
        botType = RobertTypes.Lookup("Generic");
        inventory = GetComponent<ItemContainer>();
        RefreshTraits();
    }

    public void SetRobertType(RobertType botType)
    {
        this.botType = botType;
        RefreshTraits();
    }

    public string GetRobertType()
    {
        return botType.name;
    }

    public void SetLevel(int level)
    {
        this.level = level;
        RefreshTraits();
    }

    private void RefreshTraits()
    {
        if (botType != null)
        {
            if (level >= 0 && level < botType.traitValues.Length)
            {
                traits["moveSpeed"] = botType.moveSpeed;
                traits["capacity"] = botType.capacity;
                traits["mineSpeed"] = botType.mineSpeed;
                traits["harvestMultiplier"] = botType.harvestMultiplier;
                traits["sellValueMultiplier"] = botType.sellValueMultiplier;
                if (botType.upgradeTrait != "None")
                {
                    traits[botType.upgradeTrait] = botType.traitValues[level];
                }
            }
        }
    }

    public ItemGroup GetUpgradeRequirements()
    {
        if (level < 0 || level >= botType.upgradeMats.Count)
        {
            return null;
        }
        else
        {
            return new ItemGroup(botType.upgradeMats[level]);
        }
    }

    public void Upgrade()
    {
        if (level < botType.upgradeMats.Count)
        {
            if (inventory.RemoveItems(GetUpgradeRequirements()))
            {
                level += 1;
                RefreshTraits();
            }
        }
    }

    public float GetTrait(string trait)
    {
        return traits[trait];
    }
    public void SetTrait(string trait, float value) {
        traits[trait] = value;
    }
}
