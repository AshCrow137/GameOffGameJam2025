using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BootManager : MonoBehaviour
{

    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private HexTilemapManager hexTilemapManager;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private TurnManager turnManager;
    [SerializeField]
    private BuildingManager buildingManager;
    [SerializeField]
    private CityManager cityManager;

    [SerializeField]
    private BaseKingdom[] kingdoms;

    [SerializeField]
    private CityUI cityUI;
    [SerializeField]
    private GlobalVisionManager globalVisionManager;
    //[SerializeField]
    //private ProductionQueueUI productionQueueUI;
    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private UnitSpawner unitSpawner;
    [SerializeField]
    private GamePlayEventManager gameplayEventManager;
    [SerializeField]
    private GameManager gameManager;



    [SerializeField]
    private AIController AIController;
    [SerializeField]
    private InventoryUIToggle inventoryUIToggle;
    [SerializeField]
    private InventoryItemDragged draggedInventorySlot;

    void Start()
    {
        cameraController?.Initialize();
        globalVisionManager?.Initialize();
        hexTilemapManager?.Initialize();

        UIManager?.Initialize();
        buildingManager?.Instantiate();

        cityManager?.Instantiate();
        gameManager?.Initialize();

        
        //grid units and kingdoms should initialize after hexTilemapManager
        foreach (BaseKingdom kingdom in kingdoms)
        {
            kingdom.Initialize();
        }
        //input manager should be initialized after kingdoms
        inputManager?.Initialize();
        cityUI?.Instantiate();
        unitSpawner?.Instantiate();
        //GamePlayEventManager must be initialize first of TurnManager
        gameplayEventManager?.Initialize();
        turnManager?.Initialize();


        AIController?.Initialize();
        draggedInventorySlot?.Instantiate();
        inventoryUIToggle?.Instantiate();
    }
}
