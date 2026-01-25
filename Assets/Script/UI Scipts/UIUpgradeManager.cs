using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIUpgradeStruct
{
    [SerializeField]
    public GameObject upgradePanel;
    [SerializeField]
    public TextMeshProUGUI upgradeName;
    [SerializeField]
    public TextMeshProUGUI upgradeDescription;
    [SerializeField]
    public Image UpgradeIcon;
}

public class UIUpgradeManager : MonoBehaviour
{
    public static UIUpgradeManager Instance;

    public GameObject upgradePanel;

    public List<UIUpgradeStruct> UpgradePanels;
    private List<Upgrade> UpgradeOptions;

    public void Initialize()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;

        HidePanel();
    }

    public void ShowPanel()
    {
        upgradePanel.SetActive(true);
    }

    public void HidePanel()
    {
        upgradePanel.SetActive(false);
        foreach(UIUpgradeStruct panel in UpgradePanels)
        {
            panel.upgradePanel.SetActive(false);
        }
    }

    public void ShowUpgradeOptions(List<Upgrade> options)
    {
        Debug.Log($"Option Count: {options.Count}");
        UpgradeOptions = options;
        for (int i = 0; i < options.Count; i++)
        {
            UpgradePanels[i].upgradePanel.SetActive(true);
            UpgradePanels[i].upgradeName.text = options[i].upgradeName;
            UpgradePanels[i].upgradeDescription.text = options[i].upgradeDescription;
            UpgradePanels[i].UpgradeIcon.sprite = options[i].upgradeIcon;
        }
        ShowPanel();
    }

    public void UpgradeSelected(GameObject UpgradeName)
    {
        string upgradeName = UpgradeName.GetComponent<TextMeshProUGUI>().text;
        Debug.Log($"Upgrade Setected Name: {upgradeName}");

        for(int i = 0; i < UpgradeOptions.Count; i++)
        {
            if (UpgradeOptions[i].upgradeName == upgradeName)
            {
                UpgradeSystemManager.currentUnit.AddHability(UpgradeOptions[i]);
                HidePanel();
                return;
            }
        }
        HidePanel();
    }
}
