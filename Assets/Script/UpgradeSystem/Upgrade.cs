using UnityEngine;

public class Upgrade : ScriptableObject
{
    [SerializeField]
    public string upgradeName;
    [SerializeField]
    public string upgradeDescription;
    [SerializeField]
    public Sprite upgradeIcon;
    [SerializeField]
    public int minimumLevelRequirement;
    [SerializeField]
    public int upgradeLimitLevel;        //-1 for unlimited
    [SerializeField]
    public bool isBehaviour;

    private int currentUpgradeLevel = 0;

    private bool CanIncrementUpgrade(Upgrade upgrade)
    {
        return upgradeLimitLevel == -1 || currentUpgradeLevel < upgradeLimitLevel;
    }

    public virtual void ChooseUpgrade(Upgrade upgradeChoosed)
    {
        if (CanIncrementUpgrade(upgradeChoosed))
        {
            Debug.Log("Choosing upgrade: " + upgradeName);
            
        }
        else
        {
            Debug.LogWarning("Upgrade limit reached for " + upgradeName);
        }
    }

    public virtual void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {

    }

    public virtual void IncreaseLevel(Upgrade upgrade)
    {
        upgrade.currentUpgradeLevel++;
    }

    public int GetCurrentUpgradeLevel(Upgrade upgrade)
    {
        return upgrade.currentUpgradeLevel;
    }
}
