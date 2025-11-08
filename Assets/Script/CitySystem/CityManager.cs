using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages city placement and operations on the grid
/// Usage: see testPlaceBuilding function for usage
/// </summary>
public class CityManager : MonoBehaviour
{
    // Dictionary to store city data per position
    private Dictionary<Vector3Int, GridCity> cities = new Dictionary<Vector3Int, GridCity>();
    public static CityManager Instance { get; private set; }

    [SerializeField]
    private CityData cityData;
    [SerializeField]
    private GameObject cityPrefab;
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private Resource resourceManager;
    public void Instantiate()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// Places a city at the specified grid position
    /// </summary>
    /// <param name="cityData">The city data to use for creating the city</param>
    /// <param name="gridPosition">The position on the grid (Vector3Int)</param>
    public GridCity PlaceCity(CityData cityData, Vector3Int gridPosition)
    {
        if (cityData == null)
        {
            Debug.LogError("Please assign city data");
        }

        // Create a new city instance from the city data
        //City newCity = new City(cityData, gridPosition);
        GameObject newCityObject = Instantiate(cityPrefab,HexTilemapManager.Instance.GetMainTilemap().CellToWorld(gridPosition),Quaternion.identity);
        GridCity newCity = newCityObject.GetComponent<GridCity>();
        newCity.InstantiateCity(cityData, gridPosition, unitOwner);
        // TODO: JUST FOR TESTING. REMOVE LATER AND ADD LOGIC FOR FINDING RESOURCE PER CITY
        newCity.resourceGainPerTurn = new Dictionary<ResourceType, int>
        {
            { ResourceType.Resource1, 10 },
            { ResourceType.Resource2, 10 }
        };

        cities[gridPosition] = newCity;
        tilemap.RefreshTile(gridPosition);
        return newCity;
    }

    /// <summary>
    /// Gets the sprite of the city at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>The sprite of the city at that position, or null if no city exists</returns>
    public Sprite GetCitySprite(Vector3Int position)
    {
        if (cities.TryGetValue(position, out GridCity city))
        {
            return city.sprite;
        }

        return null;
    }

    /// <summary>
    /// Gets the city at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>The city at that position, or null if no city exists</returns>
    public GridCity GetCity(Vector3Int position)
    {
        if (cities.TryGetValue(position, out GridCity city))
        {
            return city;
        }

        return null;
    }

    /// <summary>
    /// Test function for city system.
    /// Places the assigned city at the mouse position
    /// </summary>
    public void TestPlaceCity()
    {
        if(!ToggleManager.Instance.GetToggleState(ToggleUseCase.CityPlacement)) return;
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if (!CanCityBePlaced(mousePosition))
            return;

        PlaceCity(cityData, mousePosition);

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);
    }

    public void PlaceCityAtMousePosition(BaseKingdom kingdom)
    {
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if (!CanCityBePlaced(mousePosition, kingdom))
            return;

        GridCity newCity = PlaceCity(cityData, mousePosition);
        kingdom.AddCityToKingdom(newCity);

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);
    }

    private bool CanCityBePlaced(Vector3Int position, BaseKingdom kingdom)
    {
        if (!kingdom.IsTileVisible(position))
        {
            Debug.LogWarning("Cannot place city on a tile that is not visible to the kingdom.");
            return false;
        }
        return CanCityBePlaced(position);
    }

    /// <summary>
    /// Checks if a city can be placed at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>True if the city can be placed at the specified position, false otherwise</returns>
    private bool CanCityBePlaced(Vector3Int position)
    {
        if (HexTilemapManager.Instance.GetTileState(position) != TileState.Water && HexTilemapManager.Instance.GetTileState(position) != TileState.Land)
        {
            Debug.LogWarning("Tile state: " + HexTilemapManager.Instance.GetTileState(position));
            return false;
        }
        
        return true;
    }


    /// <summary>
    /// Called at the start of each turn to accumulate resources from all cities
    /// </summary>
    public void StartTurn()
    {
        AccumulateResources();
    }

    /// <summary>
    /// Accumulates resources from all cities and adds them to the resource manager
    /// </summary>
    private void AccumulateResources()
    {
        if (resourceManager == null)
        {
            Debug.LogWarning("Resource manager not assigned to CityManager");
            return;
        }

        // Iterate through all cities
        foreach (var cityEntry in cities)
        {
            GridCity city = cityEntry.Value;
            
            // Skip if city has no resources to generate
            if (city.resourceGainPerTurn == null || city.resourceGainPerTurn.Count == 0)
            {
                continue;
            }

            // Add resources from this city to the resource manager
            resourceManager.AddResource(city.resourceGainPerTurn);
        }
    }
    /// <summary>
    /// Gets all cities currently on the map
    /// </summary>
    /// <returns>Dictionary of all cities with their positions</returns>
    public Dictionary<Vector3Int, GridCity> GetAllCities()
    {
        return new Dictionary<Vector3Int, GridCity>(cities);
    }

    /// <summary>
    /// Removes a city at the specified position
    /// </summary>
    /// <param name="position">The grid position of the city to remove</param>
    /// <returns>True if a city was removed, false otherwise</returns>
    public bool RemoveCity(Vector3Int position)
    {
        if (cities.Remove(position))
        {
            tilemap.RefreshTile(position);
            return true;
        }
        return false;
    }
    public bool AddCity(Vector3Int position,GridCity city)
    {
        if(!cities.ContainsKey(position))
        {
            cities.Add(position, city);
            tilemap.RefreshTile(position);
            return true;
        }
        return false;
    }
    // Unit Spawning logic. Does it really belong here?

    [SerializeField]
    private GameObject unitPrefab;
    [SerializeField]
    private BaseKingdom unitOwner;

    public void SpawnUnitAtMousePosition(City city)
    {
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        SpawnUnit(city, mousePosition);
    }

    public void SpawnUnit(City city, Vector3Int position)
    {

        if (!CanUnitBePlaced(city, position))
            return;
        //instantiate gameobject
        GameObject unit = Instantiate(unitPrefab, HexTilemapManager.Instance.GetMainTilemap().CellToWorld(position), Quaternion.identity);
        unit.GetComponent<BaseGridUnitScript>().Initialize(unitOwner);
        HexTilemapManager.Instance.SetTileState(position, TileState.OccupiedByUnit);
    }

    private bool CanUnitBePlaced(City city, Vector3Int position)
    {
        HexTilemapManager tileManager = HexTilemapManager.Instance;
        if (tileManager.GetDistanceInCells(city.position, position) > city.unitSpawnRadius)
        {
            Debug.LogWarning("Unit spawn position is outside city vision radius.");
            return false;
        }

        if (tileManager.GetTileState(position) != TileState.Water && tileManager.GetTileState(position) != TileState.Land)
        {
            Debug.LogWarning("Tile state: " + tileManager.GetTileState(position));
            return false;
        }

        return true;
    }
    
    // public void SpawnBuilding(City city, Vector3Int position, BuildingData buildingData)
    // {
    //     if (!CanCityBePlaced(position))
    //         return;
    //     // Logic to place building in the city
    //     city.AddBuilding(buildingData);
    //     HexTilemapManager.Instance.SetTileState(position, TileState.OccuppiedByBuilding);
    // }

}

