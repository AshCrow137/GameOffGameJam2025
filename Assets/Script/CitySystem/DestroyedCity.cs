using UnityEngine;

public class DestroyedCity : GridCity
{
    [SerializeField]
    private GameObject playerCityPrefab;
    public override void Death()
    {
        base.Death();
        DrownCity();

    }
    public override void Initialize(BaseKingdom owner)
    {
        Owner = owner;
        hTM = HexTilemapManager.Instance;
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
        gridPosition = HexTilemapManager.Instance.WorldToCellPos(transform.position);
        transform.position = hTM.CellToWorldPos(gridPosition);
        hTM.PlaceCityOnTheTile(GetCellPosition(), this);
        if (CameraController.instance)
        {
            CameraArm = CameraController.instance.gameObject.transform;
        }
        entityVision = GetComponent<EntityVision>();
        if (entityVision == null)
        {
            entityVision = gameObject.AddComponent<EntityVision>();
        }
        entityVision.Initialize(this);
    }
    protected override void OnStartTurn(BaseKingdom entity)
    {
        base.OnStartTurn(entity);

        if (hTM.GetTileState(GetCellPosition()) == TileState.Water)
        {
            DrownCity();

        }
    }
    public virtual void DrownCity()
    {

        GameObject newCity = Instantiate(playerCityPrefab, transform.position, Quaternion.identity);
        GridCity gridCity = newCity.GetComponent<GridCity>();
        gridCity.Initialize(PlayerKingdom.Instance);
        hTM.RemoveCityOnTile(GetCellPosition());
        gameObject.SetActive(false);
    }
    public override void OnEntitySelect(BaseKingdom selector)
    {

    }
    public override void OnEntityDeselect()
    {

    }
}
