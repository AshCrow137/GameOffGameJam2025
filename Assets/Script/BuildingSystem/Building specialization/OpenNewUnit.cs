using UnityEngine;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;

public class OpenNewUnit : GridBuilding
{
    [Header("UnitToUpdate")]
    [SerializeField]
    private string unitName;
    private GameObject btn;

    public void OnEnable()
    {
        var allUI = Resources.FindObjectsOfTypeAll<GameObject>()
        .Where(go => go.CompareTag("UI") 
                && go.name == unitName 
                && go.scene.isLoaded);

        btn = allUI.FirstOrDefault();

        if (btn != null)
        {
            btn.SetActive(true);
            Debug.Log($"UI объект '{unitName}' найден и активирован!");
        }
        else
        {
            Debug.LogWarning($"UI объект '{unitName}' на сцене не найден!");
        }
    }

}
