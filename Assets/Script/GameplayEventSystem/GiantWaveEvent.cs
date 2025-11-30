using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "GiantWaveEvent", menuName = "BaseGameplayEvent/GiantWaveEvent")]
public class GiantWaveEvent : BaseGameplayEvent
{
    [SerializeField]
    private GameObject waveSprite;
    private Animator animator;
    private CancellationTokenSource cancellationTokenSource;
    public override void ExecuteEvent(BaseKingdom kingdom)
    {
        
        HexTilemapManager hexManager = HexTilemapManager.Instance;
        
        // Step 1 & 2: Find all land tiles adjacent to water tiles
        List<Vector3Int> coastalTiles = hexManager.GetAllCoastalLandTiles();
        
        if (coastalTiles.Count == 0)
        {
            Debug.LogWarning("No coastal tiles found for Giant Wave event");
            return;
        }
        
        // Step 3: Choose a random coastal land tile as the origin
        Vector3Int origin = coastalTiles[Random.Range(0, coastalTiles.Count)];
        
        Debug.Log($"Giant Wave origin at: {origin}");

        List<TileState> allStates = new List<TileState>
        {
            TileState.Land,
            TileState.OccuppiedByBuilding,
            TileState.OccupiedByUnit,
            TileState.OccupiedByCity
        };
        
        // Step 4: Get all tiles within radius 3 from origin
        List<Vector3Int> affectedTiles = hexManager.GetCellsInRange(origin, 3, allStates);
        if(kingdom.GetVisionManager().IsInGreyFog(origin) || kingdom.GetVisionManager().IsInNoFog(origin))
        {
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                UIManager.Instance.DisableMainCanvas();
                VisibleWaveAsync(affectedTiles, origin);
            }
            catch
            {
                CameraController.instance.UnlockCameraMovement();
                UIManager.Instance.EnableMainCanvas();
            }
           
            cancellationTokenSource.Cancel();
        }
        else
        {
            InvisibleWave(affectedTiles, origin);
        }
        
        
    }
    private async void VisibleWaveAsync(List<Vector3Int> affectedTiles,Vector3Int origin)
    {
        HexTilemapManager hexManager = HexTilemapManager.Instance;
        List<Vector3Int> adjWaterTiles = hexManager.GetCellsInRange(origin, 2, new List<TileState> { TileState.Water });
        Vector3Int WaveStartPos = origin;
        if(adjWaterTiles.Count > 0)
        {
            WaveStartPos = adjWaterTiles[Random.Range(0,adjWaterTiles.Count)];
        }
        GameObject waveClone = Instantiate(waveSprite,hexManager.CellToWorldPos(WaveStartPos),Quaternion.identity);
        waveClone.SetActive(true);
        animator = waveClone.GetComponent<Animator>();
        animator.Play("WaveStart");
        
        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("WaveFinish"))
        {
            CameraController.instance.SmoothMoveCameraToPosition(hexManager.CellToWorldPos(origin));
            waveClone.transform.position = Vector3.Lerp(waveClone.transform.position, hexManager.CellToWorldPos(origin), Time.deltaTime);
            waveClone.transform.localRotation = Quaternion.Euler(new Vector3(0,0,CameraController.instance.GetCameraArm().rotation.eulerAngles.z));
            await Task.Yield();
        }
        // Step 5: Process each tile in radius
        List<BaseGridEntity> entitiesToRemove = new List<BaseGridEntity>();
        foreach (Vector3Int tilePos in affectedTiles)
        {
            if (tilePos == new Vector3Int(6, 12, 0))
            {
                Debug.Log("Hey");
            }
            // Check each entity and destroy if it cannot stand on water
            List<BaseGridEntity> entities = hexManager.FindAllEntitiesAtPosition(tilePos);

            foreach (BaseGridEntity entity in entities)
            {

                if (!entity.GetCanStandOnTiles().Contains(TileState.Water))
                {
                    Debug.Log($"Giant Wave destroying {entity.gameObject.name} at {tilePos}");
                    //entity.Death();
                    entitiesToRemove.Add(entity);
                }
            }

            // Convert tile to water
            hexManager.ChangeTile(tilePos, TileState.Water);
        }
        foreach(BaseGridEntity entity in entitiesToRemove)
        {
            entity.Death();
        }

        // Step 6: Show UI message for player kingdom\
        UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");

        // if (kingdom is PlayerKingdom)
        // {
        //     UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");
        // }

        Debug.Log($"Giant Wave event completed. Affected {affectedTiles.Count} tiles.");
        Destroy(waveClone);
        CameraController.instance.UnlockCameraMovement();
        UIManager.Instance.EnableMainCanvas();
    }
    private  void InvisibleWave(List<Vector3Int> affectedTiles,Vector3Int origin)
    {
        HexTilemapManager hexManager = HexTilemapManager.Instance;
        

        // Step 5: Process each tile in radius

            // Check each entity and destroy if it cannot stand on water
        List<BaseGridEntity> entitiesToRemove = new List<BaseGridEntity>();
        foreach (Vector3Int tilePos in affectedTiles)
        {
            if (tilePos == new Vector3Int(-4, -4, 0))
            {
                Debug.Log("Hey");
            }
            // Check each entity and destroy if it cannot stand on water
            foreach (BaseGridEntity entity in hexManager.FindAllEntitiesAtPosition(tilePos))
            {
                if (!entity.GetCanStandOnTiles().Contains(TileState.Water))
                {
                    Debug.Log($"Giant Wave destroying {entity.gameObject.name} at {tilePos}");
                    //entity.Death();
                    entitiesToRemove.Add(entity);
                }
            }

            // Convert tile to water
            hexManager.ChangeTile(tilePos, TileState.Water);
        }
        foreach(BaseGridEntity entity in entitiesToRemove)
        {
            entity.Death();
        }

            // Convert tile to water

        

        // Step 6: Show UI message for player kingdom\
        UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");

        // if (kingdom is PlayerKingdom)
        // {
        //     UIManager.Instance.ShowGamePlayEvent("A Giant Wave has struck!");
        // }

        Debug.Log($"Giant Wave event completed. Affected {affectedTiles.Count} tiles.");


    }
    
}


