using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>
/// This class handles which item is selected
/// </summary>
public class SelectionManager : MonoBehaviour
{

    // [SerializeField]
    // private ToggleManager toggleManager;
    [SerializeField]
    private CityManager cityManager;

    [SerializeField]
    private Tilemap tilemap;
    [SerializeField] private Camera mainCamera;

    public static SelectionManager Instance { get; private set; }

    public void Instantiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Vector3Int selectedPosition;//infinite
    public City selectedCity = null;
    public BaseGridUnitScript selectedUnit = null;

    SelectionType selectedItemType;
    private bool tapHeld = false;

    public void SelectCity(City city)
    {
        selectedCity = city;
        selectedUnit = null;
        selectedItemType = SelectionType.City;
        CityUI.Instance.OnCitySelected(city);
        Debug.Log("City selected: " + city.name);
    }

    public void SelectUnit(BaseGridUnitScript unit)
    {
        selectedUnit = unit;
        selectedCity = null;
        selectedItemType = SelectionType.Unit;
        CityUI.Instance.OnCityDeselected();
        Debug.Log("Unit selected: " + unit.name);
    }

    public void Deselect()
    {
        selectedCity = null;
        selectedUnit = null;
        selectedItemType = SelectionType.None;
        CityUI.Instance.OnCityDeselected();
        Debug.Log("Deselected");
    }

    // public void OnTapRelease()
    // {
    //     Debug.Log("Tap released at time: " + Time.time  );
    //     tapHeld = false;
    // }

    //public void SelectItem(CallbackContext context)
    //{
    //    if (!context.performed) return;
    //    if (CityUI.Instance.cityMenuMode != CityMenuMode.None) return;

    //    Vector2 mousePos = Mouse.current.position.ReadValue();
    //    Ray ray = mainCamera.ScreenPointToRay(mousePos);

    //    // Cast a 3d ray onto a 2d plane and get all hits
    //    // Note: composite collider2d with outlines as geometry type does not work because that creates an edge collider with no area.
    //    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray, Mathf.Infinity);

    //    if (hits.Length == 0)
    //        return;

    //    // Prioritize unit selection - check all hits for units first
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        if (hit.collider.CompareTag("Unit"))
    //        {
    //            BaseGridUnitScript unit = hit.collider.GetComponent<BaseGridUnitScript>();
    //            if (unit != null)
    //            {
    //                SelectUnit(unit);
    //                return;
    //            }
    //        }
    //    }

    //    // If no unit was found, check for cities
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        selectedPosition = tilemap.WorldToCell(hit.point);
    //        City city = cityManager.GetCity(selectedPosition);
    //        if (city != null)
    //        {
    //            SelectCity(city);
    //            return;
    //        }
    //    }
    //    Deselect();


    //    // if(CityUI.Instance.cityMenuMode == CityMenuMode.None)
    //    // {

    //    // }

    //    // If nothing selectable was found, optionally deselect
    //    // Deselect();
    //}
}