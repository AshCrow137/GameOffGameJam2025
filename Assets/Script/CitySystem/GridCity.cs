using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class GridCity : BaseGridEntity, IDamageable
{

    [Header("Production Data")]
    [SerializeField] private int productGold = 5;
    [SerializeField] private int productMagic = 5;
    [SerializeField] private int productMaterial = 1;
    [SerializeField] private GameObject destroyedCityPrefab;
    Dictionary<ResourceType, int> product = new Dictionary<ResourceType, int>();

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
        hTM.PlaceCityOnTheTile(GetCellPosition(), this);
        owner.AddCityToKingdom(this);
        if (productGold > 0) product.Add(ResourceType.Gold, productGold);
        if (productMagic > 0) product.Add(ResourceType.Magic, productMagic);
        if (productMaterial > 0) product.Add(ResourceType.Materials, productMaterial);
        HPImage.fillAmount = (float)CurrentHealth / Health;
    }
    public void OnBuildingConstructed(GridBuilding building)
    {
        Health += (int)building.HpForCity;
        CurrentHealth += (int)building.HpForCity;
        HPImage.fillAmount = (float)CurrentHealth / Health;
    }

    protected override void OnEndTurn(BaseKingdom entity)
    {
        base.OnEndTurn(entity);
        bCanSpawnUnits = true;
        if (entity == Owner)
        {
            GetComponent<CityProductionQueue>().OnTurnEnd();
        }

    }
    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);
        if (entity != Owner) { return; }


        Owner.Resources().AddAll(product);
    }

    public void InstantiateCity(CityData cityData, Vector3Int position, BaseKingdom owner)
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
        UIManager.Instance.ActivateUnitProductionPanel(this);
        ProductionQueueUI.Instance.UpdateUI(productionQueue);

    }
    public override void OnEntityDeselect()
    {
        base.OnEntityDeselect();
        Debug.Log($"Deselect {this.name} city");
        UIManager.Instance.DeactivateUnitProductionPanel();
    }

    public void TryToSpawnUnitInCity(GameObject unitPrefab)
    {
        if (bCanSpawnUnits)
        {
            List<Vector3Int> possiblePositions = HexTilemapManager.Instance.GetCellsInRange(gridPosition, 1, unitPrefab.GetComponent<BaseGridUnitScript>().GetPossibleSpawnTiles());
            if (possiblePositions.Count <= 0)
            {
                UIManager.Instance.ShowMessageText($"This have no available tiles to spawn {unitPrefab.GetComponent<BaseGridEntity>().GetEntityDisplayName()}!");
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
            UIManager.Instance.ShowMessageText($"This city already placed a unit this turn!");
        }

    }
    public override void TakeDamage(int amount, BaseGridUnitScript attacker, bool retallitionAttack, DamageType damageType)
    {
        base.TakeDamage(amount, attacker, retallitionAttack, damageType);
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
        Dictionary<Vector3Int, GridBuilding> buildingsToDestroy = new Dictionary<Vector3Int, GridBuilding>(buildings);
        foreach (KeyValuePair<Vector3Int, GridBuilding> pair in buildingsToDestroy)
        {
            pair.Value.Death();
        }
        List<Production> productions = productionQueue?.GetTotalProduction();
        if (productions != null)
        {
            foreach (Production production in productions)
            {
                productionQueue.RemoveProduction(production);
            }

        }

        base.Death();
        hTM.RemoveCityOnTile(GetCellPosition());
        CityManager.Instance.RemoveCity(GetCellPosition());
        GetComponent<EntityVision>()?.OnDeath();
        Owner.RemoveCityFromKingdom(this);
        if (Owner is AIKingdom)
        {
            GameObject destroyedCity = Instantiate(destroyedCityPrefab, transform.position, Quaternion.identity);
            DestroyedCity script = destroyedCity.GetComponent<DestroyedCity>();
            script.Initialize(NeitralKingdom.Instance);
        }

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
