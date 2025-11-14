using UnityEngine;
using System.Linq;
public class OpenNewUnit : GridBuilding
{
    [Header("UnitToUbdate")]
    [SerializeField]
    private string unitName;
    private GameObject btn;
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        Debug.Log($"Open new unit {unitName}");
        var all = Resources.FindObjectsOfTypeAll<GameObject>();
        btn = all.FirstOrDefault(obj =>
            obj.CompareTag("UI") &&
            obj.name == "Eldritch" &&
            obj.scene.isLoaded
        );

        if (btn != null)
            btn.SetActive(true);

    }
    // public void OnDisable()
    // {
    //     if (btn != null)
    //     {
    //         btn.SetActive(false);
    //     }
    // }
}