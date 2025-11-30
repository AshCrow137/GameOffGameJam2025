using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIElementHints : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private string HintText;

    private GameObject hintObject;
    private bool bShowHint = false;
    private void OnEnable()
    {
        hintObject = UIManager.Instance.GetHintObject();
    }
    public string GetHintText()
    {
        return HintText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hintObject.SetActive(true);
        hintObject.GetComponentInChildren<TMP_Text>().text = HintText;
        bShowHint = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hintObject.SetActive(false);
        bShowHint = false;
    }
    private void LateUpdate()
    {
        if (bShowHint)
        {
            hintObject.transform.position = InputManager.instance.GetMousePosVector2();
        }
    }
}
