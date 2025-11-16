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
        less10,
        less25,
        less35,
        almostDefeat
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

public enum Fog
{
    None,   // No fog - entity fully visible
    Grey,   // Grey fog - visibility depends on entity settings
    Black   // Black fog - entity hidden
}

public enum ProductionType
{
    None,
    Unit,
    Building
}
public enum MarkerColor
{
    Red,
    Blue,
    Green,
    White
}
