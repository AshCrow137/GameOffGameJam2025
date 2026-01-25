//This file is Enum holder
using System.Collections.Generic;

public static class EnumLibrary
{
    public static List<TileState> AllTileStates = new List<TileState>()
        {
            TileState.Land,
            TileState.Water,
            TileState.OccuppiedByBuilding,
            TileState.OccupiedByUnit,
            TileState.OccupiedByCity,
            TileState.Unavailable,
            TileState.Default
        };
}
public enum TimerDirection
{
    INCREASE,
    DECREASE
}

public enum TileState
{
    Land,
    Water,
    OccupiedByUnit,
    OccuppiedByBuilding,
    OccupiedByCity,
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
public enum EntityType
{
    Infantry,
    Archer,
    Cavalry,
    Special,
    Building,
    None
}

public enum DamageType
{
    Melee,
    Ranged,
    Magic
}

public enum EffectsType
{
    Curse,
    Debuff,
    Buff
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

public enum AIUnitAction
{
    None,
    Attack,
    Move,
    UseAbility,
    Explore

}
public enum AIMacroAction
{
    None,
    Combat,
    Build,
    Explore
}
public enum AICityAction
{
    BuildProductionBuilding,
    BuildUnitTechBuilding,
    BuildHighTirUnit,
    BuildRangeUnit,
    BuildMeleeUnit,
    None
}



public enum MarkerColor
{
    Red,
    Blue,
    Green,
    White
}
public enum UnitMode
{
    None,
    Aiming,
    Casting
}

public enum GamePlayEvent
{
    GainResource,
    LostResource,
    SpawnUnit,
    SpecialEvent
}
public enum ItemType
{
    General,
    Helmet,
    Armor,
    OneHandedMelee,
    TwoHandedMelee,
    Ranged,
    Trinket,
    Shield,
}


// in futre slot type will be like Ranged Unit Main Hand, Melee unit Offhand so that we can restrict equipping items based on unit type
// or we can find some way to bring in entity type into inventory system
public enum SlotType
{
    General,
    Helmet,
    Armor,
    MainHand,
    OffHand,
    Trinket,
    Shield,
}