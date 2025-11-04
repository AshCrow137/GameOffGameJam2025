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
    private Dictionary<Vector3Int, City> cities = new Dictionary<Vector3Int, City>();
    public static CityManager Instance { get; private set; }

    [SerializeField]
    private CityData cityData;
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private Resourse resourceManager;
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
    public void PlaceCity(CityData cityData, Vector3Int gridPosition)
    {
        if (cityData == null)
        {
            Debug.LogError("Please assign city data");
        }

        // Create a new city instance from the city data
        City newCity = new City(cityData, gridPosition);

        // TODO: JUST FOR TESTING. REMOVE LATER AND ADD LOGIC FOR FINDING RESOURCE PER CITY
        newCity.resourceGainPerTurn = new Dictionary<ResourceType, int>
        {
            { ResourceType.Resource1, 10 },
            { ResourceType.Resource2, 10 }
        };

        cities[gridPosition] = newCity;
        tilemap.RefreshTile(gridPosition);
    }

    /// <summary>
    /// Gets the sprite of the city at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>The sprite of the city at that position, or null if no city exists</returns>
    public Sprite GetCitySprite(Vector3Int position)
    {
        if (cities.TryGetValue(position, out City city))
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
    public City GetCity(Vector3Int position)
    {
        if (cities.TryGetValue(position, out City city))
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
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if (!CanCityBePlaced(mousePosition))
            return;

        PlaceCity(cityData, mousePosition);

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.Occupied);
    }

    /// <summary>
    /// Checks if a city can be placed at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>True if the city can be placed at the specified position, false otherwise</returns>
    private bool CanCityBePlaced(Vector3Int position)
    {
        if (HexTilemapManager.Instance.GetTileState(position) != TileState.Available)
        {
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
            City city = cityEntry.Value;
            
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
    public Dictionary<Vector3Int, City> GetAllCities()
    {
        return new Dictionary<Vector3Int, City>(cities);
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

    public void SpawnUnit(){
        // to be done
    }

}

