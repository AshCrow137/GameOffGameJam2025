using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
    
    //[SerializeField]
    //private Resource resourceManager;

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
        //GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
    }

    /// <summary>
    /// Places a building at the specified grid position
    /// </summary>
    /// <param name="building">The building object to place</param>
    /// <param name="gridPosition">The position on the grid (Vector3Int)</param>
    public GameObject PlaceBuilding(Building building, Vector3Int gridPosition)
    {
        if (building == null)
        {
            return null;
        }
        GameObject buildingPreview = Instantiate(building.buildingPrefab, HexTilemapManager.Instance.CellToWorldPos(gridPosition), Quaternion.identity);
        buildingPreview.GetComponent<GridBuilding>().Initialize(building.ownerCity.GetOwner());
        building.ownerCity.buildings[gridPosition] = buildingPreview.GetComponent<GridBuilding>();
        building.ownerCity.OnBuildingConstructed(buildingPreview.GetComponent<GridBuilding>());
        building.buildingPlacementEvent.Post(gameObject);
        return buildingPreview;
    }

    private GameObject PlaceQueuedBuilding(Building building, BaseKingdom owner, Vector3Int position)
    {
        GameObject buildingGO = Instantiate(building.buildingPrefab, HexTilemapManager.Instance.CellToWorldPos(position), Quaternion.identity);
        buildingGO.GetComponent<GridBuilding>().InitializeBase(owner);
        foreach (SpriteRenderer sr in buildingGO.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = Color.gray;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        }
        //find canvas among children and disable it
        // Canvas canvas = buildingGO.GetComponentInChildren<Canvas>();
        // if (canvas != null)
        // {
        //     canvas.enabled = false;
        // }
        return buildingGO;
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
    // public void TestPlaceBuilding()
    // {
    //     if (!ToggleManager.Instance.GetToggleState(ToggleUseCase.BuildingPlacement)) return;
    //     Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
    //     if (mousePosition.x == int.MaxValue) return;

    //     if (!CanBuildingBePlaced(building, mousePosition)) return;

    //     if (building.duration > 0)
    //     {
    //         Debug.Log($"Starting construction of {building.buildingName} at {mousePosition}. Will complete in {building.duration} turns.");
    //         StartBuildingConstruction(building, mousePosition);
    //     }
    //     else
    //     {
    //         Debug.Log($"Placing {building.buildingName} at {mousePosition}.");
    //         PlaceBuilding(building, mousePosition);
    //     }

    //     HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);


    // }

    public bool CheckAndStartConstruction(GridCity city, Building building, Vector3Int position)
    {
        if (!CanBuildingBePlaced(city, building, position)) return false;
        Resource kingdomResource = city.GetOwner().Resources();
        kingdomResource.SpendResource(building.resource);
        building.SetOwnerCity(city);
        HexTilemapManager.Instance.SetTileState(position, TileState.OccuppiedByBuilding);
        return true;
        // if (building.duration > 0)
        // {
        //     Debug.Log($"Starting construction of {building.buildingName} at {position}. Will complete in {building.duration} turns.");
        //     Production production = new Production(position, ProductionType.Building, building.duration, building);
        //     city.GetComponent<CityProductionQueue>().AddToQueue(production);
        // }
    }

    //private void PlaceBuildingAtPosition(GridCity city, Vector3Int position)
    //{
    //    if (!CanBuildingBePlaced(city, building, position)) return;

    //    if (building.duration > 0)
    //    {
    //        Debug.Log($"Starting construction of {building.buildingName} at {position}. Will complete in {building.duration} turns.");
    //        StartBuildingConstruction(building, position);
    //    }
    //    else
    //    {
    //        Debug.Log($"Placing {building.buildingName} at {position}.");
    //        GameObject gridBuilding = PlaceBuilding(building, position);
    //        if (gridBuilding != null)
    //        {
    //            city.buildings[position] = gridBuilding.GetComponent<GridBuilding>();
    //        }
    //    }

    //    HexTilemapManager.Instance.SetTileState(position, TileState.OccuppiedByBuilding);

    //}

    //private void PlaceBuildingAtMousePosition(GridCity city)
    //{
    //    Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
    //    if (mousePosition.x == int.MaxValue) return;
    //    PlaceBuildingAtPosition(city, mousePosition);


    //}

    private bool CanBuildingBePlaced(GridCity city, Building building, Vector3Int position)
    {
        // Check if within city boundaries
        int distanceToCity = HexTilemapManager.Instance.GetDistanceInCells(city.position, position);
        if (distanceToCity > city.unitSpawnRadius)
        {
            Debug.LogWarning($"Cannot place building outside city boundaries. Distance to city: {distanceToCity}, allowed radius: {city.unitSpawnRadius}");
            return false;
        }

        return CanBuildingBePlaced(building, position, city.GetOwner());
    }


    private bool CanBuildingBePlaced(Building building, Vector3Int position, BaseKingdom kingdom){

        if(HexTilemapManager.Instance.GetTileState(position) != TileState.Land && HexTilemapManager.Instance.GetTileState(position) != TileState.Water)
        {
            return false;
        }

        // Get building's resource requirements
        Dictionary<ResourceType, int> resourceRequirements = building.resource;
        // return true;
        Resource kingdomResource = kingdom.Resources();
        Dictionary<ResourceType, int> resultReqs = kingdomResource.HasEnough(resourceRequirements);
        if(resultReqs!=null)
        {
            foreach (var a in resultReqs)
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"not enough {a.Key} - {a.Value}");
                //Debug.Log($"not enough {a.Key} - {a.Value}");
            }
        }
        return resultReqs == null;
    }

    /// <summary>
    /// Initiates building construction that will complete after the specified duration
    /// </summary>
    /// <param name="building">The building to construct</param>
    /// <param name="position">The grid position where the building will be placed</param>
    //private void StartBuildingConstruction(Building building, Vector3Int position)
    //{

    //    // Consume resources
    //    resourceManager.SpendResource(building.resource);

    //    // Add to construction queue
    //    BuildingConstruction construction = new BuildingConstruction(building, position, building.duration);
    //    ongoingConstructions.Add(construction);

    //}

    /// <summary>
    /// Queues a building for production at mouse position
    /// </summary>
    public void QueueBuildingAtMousePosition(GridCity city,int buildingType)
    {
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue)
        {
            Debug.LogError("Invalid mouse position");
            return;
        }
        Building building = BuildingsDatas[buildingType];
        QueueBuildingAtPosition(mousePosition, city, building);

    }
    public bool QueueBuildingAtPosition(Vector3Int position,GridCity city,GridBuilding gridBuilding )
    {
        Building building = gridBuilding.GetBuilding();
        return QueueBuildingAtPosition(position, city, building);
    }

    private bool QueueBuildingAtPosition(Vector3Int position, GridCity city, Building building)
    {
        if (!CanBuildingBePlaced(city, building, position))
        {
            Debug.LogError("Building cannot be placed at this position");
            return false;
        }

        if (city.GetComponent<CityProductionQueue>() == null)
        {
            Debug.LogError("CityProductionQueue.Instance is null");
            return false;
        }
        CheckAndStartConstruction(city, building, position);
        GameObject placedBuilding = PlaceQueuedBuilding(building, city.GetOwner(), position);

        Production production = new Production(position, ProductionType.Building, building.duration, building, placedBuilding);
        if (TurnManager.instance.GetCurrentActingKingdom() is PlayerKingdom)
        {
            building.buildingPlacementEvent.Post(gameObject);
        }

        city.GetComponent<CityProductionQueue>().AddToQueue(production);
        return true;
    }



    // set tile to available, and refund resources
    public void CancelConstruction(GridCity city, Building building, Vector3Int position)
    {
        if(city == null || building == null || position == null)
        {
            Debug.LogError("Invalid parameters");
            return;
        }
        if(HexTilemapManager.Instance.GetTileState(position) != TileState.OccuppiedByBuilding)
        {
            Debug.LogError("Tile was not occupied by building when cancelling construction");
            return;
        }
        HexTilemapManager.Instance.SetTileState(position, TileState.Water);
        city.GetOwner().Resources().AddAll(building.resource);

    }


    ///// <summary>
    ///// Called at the start of each turn to progress building construction
    ///// </summary>
    //public void StartTurn()
    //{
    //    // Collect completed constructions to remove after iteration
    //    List<BuildingConstruction> completedConstructions = new List<BuildingConstruction>();

    //    // Process each ongoing construction
    //    foreach (BuildingConstruction construction in ongoingConstructions)
    //    {
    //        construction.turnsRemaining--;

    //        if (construction.turnsRemaining <= 0)
    //        {
    //            // Construction complete - place the building
    //            PlaceBuilding(construction.building, construction.position);
    //            completedConstructions.Add(construction);
    //            Debug.Log($"Construction complete! {construction.building.buildingName} placed at {construction.position}");
    //        }
    //        else
    //        {
    //            Debug.Log($"{construction.building.buildingName} at {construction.position}: {construction.turnsRemaining} turns remaining");
    //        }
    //    }

    //    // Remove completed constructions from the list
    //    foreach (BuildingConstruction construction in completedConstructions)
    //    {
    //        ongoingConstructions.Remove(construction);
    //    }

    //}
}

