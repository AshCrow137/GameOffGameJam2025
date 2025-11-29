using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.GridLayoutGroup;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner Instance { get; private set; }



    //[SerializeField]
    //private Resource resourceManager;

    [SerializeField]
    private PlayerKingdom playerKingdom;

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
    /// Queues a unit for production at mouse position
    /// </summary>
    public void QueueUnitAtMousePosition(GridCity city, GameObject unitPrefab)
    {
        Vector3Int mousePosition = HexTilemapManager.Instance.GetCellAtMousePosition();
        if (mousePosition.x == int.MaxValue)
        {
            Debug.LogError("Invalid mouse position");
            return;
        }
        if (!CanUnitBePlaced(city, unitPrefab, mousePosition))
        {
            Debug.LogError("Unit cannot be placed at this position");
            return;
        }
        

        Production production = new Production(mousePosition, ProductionType.Unit, unitPrefab.GetComponent<BaseGridUnitScript>().duration, unitPrefab);

        if (city.GetComponent<CityProductionQueue>() == null)
        {
            Debug.LogError("CityProductionQueue is null");
            return;
        }

        city.GetComponent<CityProductionQueue>().AddToQueue(production);
    }
    public void QueueUnitAtPosition(Vector3Int position,GridCity city,BaseGridUnitScript unitPrefab)
    {
        Production production = new Production(position, ProductionType.Unit, unitPrefab.GetComponent<BaseGridUnitScript>().duration, unitPrefab.gameObject);

        if (city.GetComponent<CityProductionQueue>() == null)
        {
            Debug.LogError("CityProductionQueue is null");
            return;
        }

        city.GetComponent<CityProductionQueue>().AddToQueue(production);
    }

    /// <summary>
    /// Cancels unit production and refunds resources
    /// </summary>
    public void CancelSpawning(GridCity city, GameObject unit, Vector3Int position)
    {
        if (city == null || unit == null || position == null)
        {
            Debug.LogError("Invalid parameters");
            return;
        }
        // Refund resources
        city.GetOwner().Resources().AddAll(unit.GetComponent<BaseGridUnitScript>().resource);
    }

    /// <summary>
    /// Checks if a unit can be placed at the specified position
    /// </summary>
    private bool CanUnitBePlaced(GridCity city, GameObject unitPrefab, Vector3Int position)
    {
        // Check if within city spawn radius

        List<Vector3Int> possiblePositions = HexTilemapManager.Instance.GetCellsInRange(city.position, city.unitSpawnRadius, unitPrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
        if (!possiblePositions.Contains(position))
        {
            Debug.LogWarning("Cannot place the unit at this position");
            return false;
        }
        Dictionary<ResourceType, int> resourceRequirements = unitPrefab.GetComponent<BaseGridUnitScript>().resource;
        Dictionary<ResourceType, int> resultReqs = city.GetOwner().Resources().HasEnough(resourceRequirements);
        if (resultReqs != null)
        {
            foreach (var a in resultReqs)
            {
                Debug.LogWarning($"not enough {a.Key} - {a.Value}");
            }
        }
        return resultReqs == null;
    }

    /// <summary>
    /// Checks and starts unit construction if valid
    /// </summary>
    public bool CheckAndStartUnitSpawn(GridCity city, GameObject unitPrefab, Vector3Int position)
    {
        if (!CanUnitBePlaced(city, unitPrefab, position)) return false;

        city.GetOwner().Resources().SpendResource(unitPrefab.GetComponent<BaseGridUnitScript>().resource);
        HexTilemapManager.Instance.SetTileState(position, TileState.OccupiedByUnit);

        return true;
    }

    /// <summary>
    /// Places a unit at the specified grid position
    ///// </summary>
    //public GameObject PlaceUnit(GameObject unitPrefab, Vector3Int gridPosition)
    //{
    //    BaseKingdom currentKingdom = TurnManager.instance.GetCurrentActingKingdom();
    //    return PlaceUnit(unitPrefab, gridPosition, currentKingdom);
    //}

    public GameObject PlaceUnit(GameObject unitPrefab, Vector3Int gridPosition, BaseKingdom ownerKingdom)
    {
        //if (unitPrefab == null)
        //{
        //    return null;
        //}
        GameObject unit = Instantiate(unitPrefab, HexTilemapManager.Instance.CellToWorldPos(gridPosition), Quaternion.identity);
        //HexTilemapManager.Instance.SetTileState(gridPosition, TileState.OccupiedByUnit);
        Seeker seeker = unit.GetComponent<Seeker>();
        var r = seeker.traversableTags;

        unit.GetComponent<BaseGridUnitScript>().Initialize(ownerKingdom);
        if(ownerKingdom is AIKingdom)
        {
            AIKingdom aiownerKingdom = (AIKingdom) ownerKingdom;
            if(aiownerKingdom.GetCurrentMadnessEffect().DecreaseSpeed)
            {
                unit.GetComponent<BaseGridUnitScript>().DecreaseSpeedForFirstTurn();
            }
           
        }
        //unit.GetComponent<BaseGridUnitScript>().Initialize(playerKingdom);

        ownerKingdom.AddUnitToKingdom(unit.GetComponent<BaseGridUnitScript>());
        UIManager.Instance.UnitsInteractable(false);

        // Initialize unit if it has a GridUnit component
        // unitObject.GetComponent<GridUnit>().Initialize(unit, playerKingdom);
        return unit;
    }
}

