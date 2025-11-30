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
        GlobalEventManager.EndTurnEvent.AddListener(OnEndTurn);
        GlobalEventManager.StartTurnEvent.AddListener(OnStartTurn);
        gridPosition = HexTilemapManager.Instance.WorldToCellPos(transform.position);
        transform.position = hTM.CellToWorldPos(gridPosition);
        if (Camera.main != null && Camera.main.transform.parent != null)
        {
            CameraArm = Camera.main.transform.parent;
        }
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
        gameObject.SetActive(false);
    }
}
