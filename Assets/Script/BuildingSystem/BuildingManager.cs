using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages building placement and operations on the grid
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

    private void Awake()
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
        PlaceBuilding(building, mousePosition);
    }
}

