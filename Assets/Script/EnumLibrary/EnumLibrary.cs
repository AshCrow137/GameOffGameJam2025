//This file is Enum holder
    public enum TimerDirection
    {
        INCREASE,
        DECREASE
    }

    public enum TileState
    {
        Land,
        OccuppiedByBuilding,
        OccupiedByUnit,
        Water,
        Unavailable,
        Default
    }

    public enum ToggleUseCase
    {
        UnitPlacement,
        BuildingPlacement,
        CityPlacement
    }

    public enum MadnessEffect
    {
        None,
        BetterDeal,
        FreeUnits,
        ImproveUnits,
        PreventAttacks
    }

public enum SelectionType
{
    None,
    City,
    Unit
}
    
    public enum CityMenuMode
    {
        None,
        SpawnUnit,
        SpawnBuilding
    }
public enum UnitType
{
    Infantry,
    Archer,
    Cavalry,
    Special
}