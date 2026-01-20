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
    public int amountToUpgrade;
    [SerializeField]
    public bool isBehaviour;

    private int currentUpgradeLevel = 0;

    private bool CanChooseUpgrade(Upgrade upgrade)
    {
        return upgradeLimitLevel == -1 || currentUpgradeLevel < upgradeLimitLevel;
    }

    public virtual void ChooseUpgrade(Upgrade upgradeChoosed)
    {
        if (CanChooseUpgrade(upgradeChoosed))
        {
            Debug.Log("Choosing upgrade: " + upgradeName);
            if (isBehaviour)
            {
                currentUpgradeLevel++;
                amountToUpgrade++;
            }
        }
        else
        {
            Debug.LogWarning("Upgrade limit reached for " + upgradeName);
        }
    }

    public virtual void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {

    }
}
