using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages building placement and operations on the grid
/// Usage: see testPlaceBuilding function for usage
/// </summary>
/// 

public class BuildingManager : MonoBehaviour
{
    // Dictionary to store building data per position
    private Dictionary<Vector3Int, Building> buildings = new Dictionary<Vector3Int, Building>();
    public static BuildingManager Instance { get; private set; }

    [SerializeField]
    private Building building;

    [SerializeField]
    private GameObject buildingPrefab;
    [SerializeField]
    private List<Building> BuildingsDatas;

    [SerializeField]
    private List<GameObject> PrefabsBuilding;

    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private Resource resourceManager;

    // List to track buildings under construction
    private List<BuildingConstruction> ongoingConstructions = new List<BuildingConstruction>();
    [SerializeField]
    private PlayerKingdom playerKngdom;

    // Nested class to track construction progress
    [System.Serializable]
    private class BuildingConstruction
    {
        public Building building;
        public Vector3Int position;
        public int turnsRemaining;
        public int buildingtype;

        public BuildingConstruction(Building building, Vector3Int position, int duration, int buildingsType)
        {
            this.building = building;
            this.position = position;
            this.turnsRemaining = duration;
            this.buildingtype = buildingsType;
        }
    }

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
    /// Places a building at the specified grid position
    /// </summary>
    /// <param name="building">The building object to place</param>
    /// <param name="gridPosition">The position on the grid (Vector3Int)</param>
    public GameObject PlaceBuilding(Building building, Vector3Int gridPosition,int buildingType)
    {
        if (building == null)
        {
            return null;
        }
        GameObject buildingPreview = Instantiate(PrefabsBuilding[buildingType], HexTilemapManager.Instance.CellToWorldPos(gridPosition), Quaternion.identity);
        buildingPreview.GetComponent<GridBuilding>().Initialize(building, playerKngdom);
        building.ownerCity.buildings[gridPosition] = buildingPreview.GetComponent<GridBuilding>();
        return buildingPreview;
    }

    /// <summary>
    /// Gets the sprite of the building at the specified position
    /// </summary>
    /// <param name="position">The grid position to check</param>
    /// <returns>The sprite of the building at that position, or null if no building exists</returns>
    public Sprite GetBuildingSprite(Vector3Int position)
    {
        if (buildings.TryGetValue(position, out Building building))
        {
            return building.sprite;
        }

        return null;
    }


    /// <summary>
    /// Test function for building system.
    /// Places the assigned building at the mouse position
    /// </summary>
    public void TestPlaceBuilding()
    {
        if (!ToggleManager.Instance.GetToggleState(ToggleUseCase.BuildingPlacement)) return;
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if (!CanBuildingBePlaced(building, mousePosition)) return;

        if (building.duration > 0)
        {
            Debug.Log($"Starting construction of {building.buildingName} at {mousePosition}. Will complete in {building.duration} turns.");
            StartBuildingConstruction(building, mousePosition,0);
        }
        else
        {
            Debug.Log($"Placing {building.buildingName} at {mousePosition}.");
            PlaceBuilding(building, mousePosition,0);
        }

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);


    }

    public void PlaceBuildingAtMousePosition(GridCity city, int buildingType)
    {
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if (!CanBuildingBePlaced(city, building, mousePosition)) return;

        if (building.duration > 0)
        {
            Debug.Log($"Starting construction of {building.buildingName} at {mousePosition}. Will complete in {building.duration} turns.");
            BuildingsDatas[buildingType].SetOwnerCity(city);
            StartBuildingConstruction(BuildingsDatas[buildingType], mousePosition,buildingType);
        }
        else
        {
            Debug.Log($"Placing {building.buildingName} at {mousePosition}.");
            GameObject gridBuilding = PlaceBuilding(BuildingsDatas[buildingType], mousePosition,buildingType);
            if( gridBuilding != null)
            {
                city.buildings[mousePosition] = gridBuilding.GetComponent<GridBuilding>();
                Debug.Log(city.buildings);
                //city.maxHP += gridBuilding.GetComponent<GridBuilding>().HpForCity;
                //Debug.Log($"!!!!!!!!!!!!CityHp how: {city.maxHP}");
            }
        }

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);

    }

    private bool CanBuildingBePlaced(GridCity city, Building building, Vector3Int position)
    {
        // Check if within city boundaries
        int distanceToCity = HexTilemapManager.Instance.GetDistanceInCells(city.position, position);
        if (distanceToCity > city.unitSpawnRadius)
        {
            Debug.LogWarning($"Cannot place building outside city boundaries. Distance to city: {distanceToCity}, allowed radius: {city.unitSpawnRadius}");
            return false;
        }

        return CanBuildingBePlaced(building, position);
    }


    private bool CanBuildingBePlaced(Building building, Vector3Int position){

        if(HexTilemapManager.Instance.GetTileState(position) != TileState.Land && HexTilemapManager.Instance.GetTileState(position) != TileState.Water)
        {
            return false;
        }

        // Get building's resource requirements
        Dictionary<ResourceType, int> resourceRequirements = building.resource;
        // return true;
        return resourceManager.HasEnough(resourceRequirements) == null;
    }

    /// <summary>
    /// Initiates building construction that will complete after the specified duration
    /// </summary>
    /// <param name="building">The building to construct</param>
    /// <param name="position">The grid position where the building will be placed</param>
    private void StartBuildingConstruction(Building building, Vector3Int position,int buildingType)
    {

        // Consume resources
        resourceManager.SpendResource(building.resource);

        // Add to construction queue
        BuildingConstruction construction = new BuildingConstruction(building, position, building.duration, buildingType);
        ongoingConstructions.Add(construction);

    }

    /// <summary>
    /// Called at the start of each turn to progress building construction
    /// </summary>
    public void StartTurn()
    {
        // Collect completed constructions to remove after iteration
        List<BuildingConstruction> completedConstructions = new List<BuildingConstruction>();

        // Process each ongoing construction
        foreach (BuildingConstruction construction in ongoingConstructions)
        {
            construction.turnsRemaining--;
            
            if (construction.turnsRemaining <= 0)
            {
                // Construction complete - place the building
                PlaceBuilding(construction.building, construction.position, construction.buildingtype);

                completedConstructions.Add(construction);
                Debug.Log($"Construction complete! {construction.building.buildingName} placed at {construction.position}");
            }
            else
            {
                Debug.Log($"{construction.building.buildingName} at {construction.position}: {construction.turnsRemaining} turns remaining");
            }
        }

        // Remove completed constructions from the list
        foreach (BuildingConstruction construction in completedConstructions)
        {
            ongoingConstructions.Remove(construction);
        }

    }
}

