using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages building placement and operations on the grid
/// Usage: see testPlaceBuilding function for usage
/// </summary>
public class BuildingManager : MonoBehaviour
{
    // Dictionary to store building data per position
    private Dictionary<Vector3Int, Building> buildings = new Dictionary<Vector3Int, Building>();
    public static BuildingManager Instance { get; private set; }

    [SerializeField]
    private Building building;
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private Resourse resourceManager;

    // List to track buildings under construction
    private List<BuildingConstruction> ongoingConstructions = new List<BuildingConstruction>();

    // Nested class to track construction progress
    [System.Serializable]
    private class BuildingConstruction
    {
        public Building building;
        public Vector3Int position;
        public int turnsRemaining;

        public BuildingConstruction(Building building, Vector3Int position, int duration)
        {
            this.building = building;
            this.position = position;
            this.turnsRemaining = duration;
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
    public void PlaceBuilding(Building building, Vector3Int gridPosition)
    {
        if (building == null)
        {
            return;
        }

        buildings[gridPosition] = building;
        tilemap.RefreshTile(gridPosition);
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
    public void TestPlaceBuilding(){
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue) return;

        if(!CanBuildingBePlaced(building, mousePosition)) return;

        if(building.duration > 0){
            Debug.Log($"Starting construction of {building.buildingName} at {mousePosition}. Will complete in {building.duration} turns.");
            StartBuildingConstruction(building, mousePosition);
        } else {
            Debug.Log($"Placing {building.buildingName} at {mousePosition}.");
            PlaceBuilding(building, mousePosition);
        }

        HexTilemapManager.Instance.SetTileState(mousePosition, TileState.OccuppiedByBuilding);


    }

    private bool CanBuildingBePlaced(Building building, Vector3Int position){

        if(HexTilemapManager.Instance.GetTileState(position) != TileState.Land){
            return false;
        }

        // Get building's resource requirements
        Dictionary<ResourceType, int> resourceRequirements = building.resource;

        return resourceManager.HasEnough(resourceRequirements);
    }

    /// <summary>
    /// Initiates building construction that will complete after the specified duration
    /// </summary>
    /// <param name="building">The building to construct</param>
    /// <param name="position">The grid position where the building will be placed</param>
    private void StartBuildingConstruction(Building building, Vector3Int position)
    {

        // Consume resources
        resourceManager.SpendResource(building.resource);

        // Add to construction queue
        BuildingConstruction construction = new BuildingConstruction(building, position, building.duration);
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
                PlaceBuilding(construction.building, construction.position);
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

