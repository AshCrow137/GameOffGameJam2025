using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridCity : BaseGridEntity
{
    public Sprite sprite;
    public Vector3Int position;

    // we can add building's relation to city later
    // public List<Building> buildings;

    public float maxHP = 100f;
    public float currentHP;

    public int visionRadius = 1;
    public int unitSpawnRadius = 1;

    public Dictionary<Vector3Int, GridBuilding> buildings = new Dictionary<Vector3Int, GridBuilding>();

    // Resources this city generates per turn
    public Dictionary<ResourceType, int> resourceGainPerTurn = new Dictionary<ResourceType, int>();
    private bool bCanSpawnUnits = true;
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        CityManager.Instance.AddCity(HexTilemapManager.Instance.WorldToCellPos(transform.position), this);
        
    }

    protected override void OnEndTurn(BaseKingdom entity)
    {
            base.OnEndTurn(entity);
            bCanSpawnUnits = true;
        
    }

    public void InstantiateCity(CityData cityData, Vector3Int position,BaseKingdom owner)
    {
        this.sprite = cityData.sprite;
        this.position = position;
        this.maxHP = cityData.maxHP;
        this.currentHP = cityData.maxHP;
        this.visionRadius = cityData.visionRadius;
        this.unitSpawnRadius = 1;
        this.Owner = owner;

        // Initialize empty resource dictionary - will be populated by other means
        this.resourceGainPerTurn = new Dictionary<ResourceType, int>();
        Owner.AddCityToKingdom(this);
        
        Initialize(Owner);
    }
    public override void OnEntitySelect(BaseKingdom selector)
    {
        base.OnEntitySelect(selector);
        Debug.Log($"Select {this.name} city");
        HPImage.color = Color.gray;
        GameplayCanvasManager.instance.ActivateUnitProductionPanel(this);
    }
    public override void OnEntityDeselect()
    {
        Debug.Log($"Deselect {this.name} city");
        HPImage.color = Owner.GetKingdomColor();
        GameplayCanvasManager.instance.DeactivateUnitProductionPanel();
    }

    public void TryToSpawnUnitInCity(GameObject unitPrefab)
    {
        if(bCanSpawnUnits) 
        {
            List<Vector3Int> possiblePositions = HexTilemapManager.Instance.GetCellsInRange(gridPosition, 1,unitPrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            if(possiblePositions.Count <= 0)
            {
                GlobalEventManager.InvokeShowUIMessageEvent($"This have no available tiles to spawn {unitPrefab.name}!");
                return;
            }
            Vector3Int randomPos = possiblePositions[Random.Range(0, possiblePositions.Count - 1)];
            GameObject newUnit = Instantiate(unitPrefab, HexTilemapManager.Instance.CellToWorldPos(randomPos), Quaternion.identity);
            BaseGridUnitScript unitScript = newUnit.GetComponent<BaseGridUnitScript>();
            Seeker seeker = newUnit.GetComponent<Seeker>();
            var r = seeker.traversableTags;
            Debug.Log(r);
            unitScript.Initialize(Owner);
            Owner.AddUnitToKingdom(unitScript);
            bCanSpawnUnits = false;
        }
        else
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"This city already placed a unit this turn!");
        }
       
    }
    
}
