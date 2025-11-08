using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayCanvasManager : MonoBehaviour
{
    public static GameplayCanvasManager instance { get; private set; }
    [SerializeField]
    private GameObject unitProductionPanel;
    [SerializeField]
    private TMP_Text messageText;
    [SerializeField]
    private float showMessageTime = 3;

    private GridCity selectedCity;
    private IEnumerator showMessageCoroutine;
    public void Initialize()
    {
        if(instance!= null)
        {
            Destroy(this);
        }
        instance = this;
    }
    public void ActivateUnitProductionPanel(GridCity city)
    {
        unitProductionPanel.SetActive(true);
        selectedCity = city;
    }
    public void DeactivateUnitProductionPanel()
    {
        unitProductionPanel.SetActive(false);
        selectedCity = null;
    }
    public void ShowMessageText(string message)
    {
        messageText.text = message;
        if(showMessageCoroutine!=null)
        {
            StopCoroutine(showMessageCoroutine);
            showMessageCoroutine = null;
        }
        showMessageCoroutine = ShowMessageCoroutine();
        StartCoroutine(showMessageCoroutine);
    }
    private IEnumerator ShowMessageCoroutine()
    {
        messageText.gameObject.SetActive(true);
        float t = showMessageTime;
        while (t>0)
        {
            t-= Time.deltaTime;
            yield return null;
        }
        messageText.gameObject.SetActive(false);

    }
    public void TryToSpawnUnit(GameObject unitPrefab)
    {
        selectedCity?.TryToSpawnUnitInCity(unitPrefab);
    }
    public void OnMouseEnterCanvasElement()
    {
        InputManager.instance.SetOnUiElement(true);
    } 
    public void OnMouseExitCanvasElement()
    {
        InputManager.instance.SetOnUiElement(false);
    }
}
