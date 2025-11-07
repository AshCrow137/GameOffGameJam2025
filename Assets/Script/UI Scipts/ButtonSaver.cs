using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSaver : MonoBehaviour
{
    public Button[] buttons;
    public int selectedIndex = 0;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    private void Update()
    {
        // Если ничего не выбрано — восстанавливаем последнюю кнопку
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
        }
    }

    public void SelectButton(int index)
    {
        selectedIndex = index;
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }
}
