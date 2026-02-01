public static class UIUtility
{

    public static GridCity selectedCity { get; private set; }
    public static BaseGridUnitScript selectedUnit { get; private set; }
    public static bool bHasSelectedEntity { get; private set; } = false;
    public static void SelectCity(GridCity newCity)
    {
        selectedCity = newCity;
        selectedCity.OnEntitySelect(PlayerKingdom.Instance);
        bHasSelectedEntity = true;
        UIManager.Instance?.OnCitySelect(newCity);
    }
    public static void SelectUnit(BaseGridUnitScript newUnit)
    {
        selectedUnit = newUnit;
        selectedUnit.OnEntitySelect(PlayerKingdom.Instance);
        bHasSelectedEntity = true;
        UIManager.Instance.OnUnitSelect(newUnit);
    }
    public static void DeselectCity()
    {
        selectedCity.OnEntityDeselect();
        selectedCity = null;
        bHasSelectedEntity = false;
    }
    public static void DeselectUnit()
    {
        selectedUnit.OnEntityDeselect();
        selectedUnit = null;
        bHasSelectedEntity = false;
    }
}
