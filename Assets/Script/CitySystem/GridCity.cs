using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridCity : BaseGridEntity,IDamageable
{
    public Sprite sprite;
    public Vector3Int position;

    // we can add building's relation to city later
    // public List<Building> buildings;


    // public int visionRadius = 1;
    public int unitSpawnRadius = 1;

    public Dictionary<Vector3Int, GridBuilding> buildings = new Dictionary<Vector3Int, GridBuilding>();

    // Resources this city generates per turn
    public Dictionary<ResourceType, int> resourceGainPerTurn = new Dictionary<ResourceType, int>();
    private bool bCanSpawnUnits = true;
    public CityProductionQueue productionQueue { get; private set; }
    public override void Initialize(BaseKingdom owner)
    {
        base.Initialize(owner);
        position = HexTilemapManager.Instance.WorldToCellPos(transform.position);
        CityManager.Instance.AddCity(HexTilemapManager.Instance.WorldToCellPos(transform.position), this);
        productionQueue = GetComponent<CityProductionQueue>();
        if (productionQueue == null)
        {
           gameObject.AddComponent<CityProductionQueue>();
            productionQueue = GetComponent<CityProductionQueue>();
        }
        productionQueue.Initialize(owner, this);
        hTM.PlaceCityOnTheTile(GetCellPosition(),this);
        owner.AddCityToKingdom(this);
    }
    public void OnBuildingConstructed(GridBuilding building)
    {
        Health +=(int) building.HpForCity;
        CurrentHealth += (int)building.HpForCity;
        HPImage.fillAmount = (float)CurrentHealth / Health;
    }

    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
        bCanSpawnUnits = true;
        if(entity==Owner)
        {
            GetComponent<CityProductionQueue>().OnTurnEnd();
        }
        
    }
    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);


    }

    public void InstantiateCity(CityData cityData, Vector3Int position,BaseKingdom owner)
    {
        this.sprite = cityData.sprite;
        this.position = position;
        // this.visionRadius = cityData.visionRadius;
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
        GameplayCanvasManager.instance.ActivateUnitProductionPanel(this);
        ProductionQueueUI.Instance.UpdateUI(productionQueue);
        
    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        Debug.Log($"Deselect {this.name} city");
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
            //lets the others UI production unavaliable
            UIManager.Instance.UnitsInteractable(false);
        }
        else
        {
            GlobalEventManager.InvokeShowUIMessageEvent($"This city already placed a unit this turn!");
        }
       
    }
    public override void TakeDamage(int amount, BaseGridUnitScript attacker, bool retallitionAttack)
    {
        base.TakeDamage(amount, attacker, retallitionAttack);
        CurrentHealth -= amount;
        Debug.Log($"{this.gameObject.name} take {amount} damage");
        HPImage.fillAmount = (float)CurrentHealth / Health;
        if (CurrentHealth <= 0)
        {


            Death();
            return;
        }
    }
    public override void Death()
    {
        base.Death();
        hTM.RemoveCityOnTile(GetCellPosition());
        GetComponent<EntityVision>().OnDeath();
        Owner.RemoveCityFromKingdom(this);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// If all tiles in 1 unit radius is either occupied by building or city or unit then unitSpawn radius = 2, 
    /// max unitSpawn radius is 5
    /// </summary>
    public void UpdateUnitSpawnRadius()
    {
        const int maxRadius = 5;
        
        // Keep expanding the radius until we find available tiles or reach max radius
        for (int checkRadius = 1; checkRadius <= maxRadius; checkRadius++)
        {
            // GetCellsInRange already filters for Land and Water tiles by default
            List<Vector3Int> cellsInRadius = HexTilemapManager.Instance.GetCellsInRange(gridPosition, checkRadius);
            
            // If we found available land/water tiles at this radius, set it and stop
            if (cellsInRadius.Count > 0)
            {
                unitSpawnRadius = checkRadius;
                return;
            }
        }
        
        // If no available tiles found even at max radius, set to max
        unitSpawnRadius = maxRadius;
    }

}
