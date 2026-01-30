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
    public int amountOfIncrement;
    [SerializeField]
    public int minimumLevelRequirement;
    [SerializeField]
    public int upgradeLimitLevel;        //-1 for unlimited
    [SerializeField]
    public bool isBehaviour;

    private int currentUpgradeLevel = 0;

    public virtual void Init(Upgrade upgrade)
    {
        this.upgradeName = upgrade.upgradeName;
        this.upgradeDescription = upgrade.upgradeDescription;
        this.upgradeIcon = upgrade.upgradeIcon;
        this.amountOfIncrement = upgrade.amountOfIncrement;
        this.minimumLevelRequirement = upgrade.minimumLevelRequirement;
        this.upgradeLimitLevel = upgrade.upgradeLimitLevel;
        this.isBehaviour = upgrade.isBehaviour;
        this.currentUpgradeLevel = 1;
    }

    private bool CanIncrementUpgrade(Upgrade upgrade)
    {
        return upgrade.upgradeLimitLevel == -1 || upgrade.GetCurrentUpgradeLevel(upgrade) < upgrade.upgradeLimitLevel;
    }

    public virtual void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        Debug.Log("In Upgrade Class");
    }

    public virtual void IncreaseLevel(Upgrade upgrade)
    {
        if (CanIncrementUpgrade(upgrade))
        {
            upgrade.currentUpgradeLevel++;
        }
    }

    public int GetCurrentUpgradeLevel(Upgrade upgrade)
    {
        return upgrade.currentUpgradeLevel;
    }

    public void ResetCurrentUpgradeLevel(Upgrade upgrade)
    {
        upgrade.currentUpgradeLevel = 0;
    }

    public int GetAmount(Upgrade upgrade)
    {
        return upgrade.amountOfIncrement;
    }
}
