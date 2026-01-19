using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
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

    private bool CanUpgrade()
    {
        return upgradeLimitLevel == -1 || currentUpgradeLevel < upgradeLimitLevel;
    }

    public virtual void ApplyUpgrade(UnitStats unitToApplyUpgrade)
    {
        if (CanUpgrade())
        {
            Debug.Log("Applying upgrade: " + upgradeName);
            Debug.Log("Upgrade description: " + upgradeDescription);
            currentUpgradeLevel++;
        }
        else
        {
            Debug.LogWarning("Upgrade limit reached for " + upgradeName);
        }
    }
}
