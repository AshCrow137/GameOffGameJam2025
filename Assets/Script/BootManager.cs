using UnityEngine;


public class BootManager : MonoBehaviour
{
        ////In this example, you have to put value in testScript directly from Inspector in Unity
        //[SerializeField] 
        //private TestScript testScript;
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
        private ToggleManager toggleManager;
    [SerializeField]
    private BaseKingdom[] kingdoms;
    [SerializeField]
    private SelectionManager selectionManager;
    [SerializeField]
    private GameplayCanvasManager gameplayCanvasManager;
    [SerializeField]
    private Resource resource;
    [SerializeField]
    private CityUI cityUI;
    [SerializeField]
    private KingdomUI kingdomUI;
    [SerializeField]
    private AddVisibleTiles addVisibleTiles;
    [SerializeField]
    private GlobalVisionManager globalVisionManager;
    [SerializeField]
    private ProductionQueueUI productionQueueUI;
    [SerializeField]
    private UIManager UIManager;
    [SerializeField]
    private UnitSpawner unitSpawner;
    [SerializeField]
    private GamePlayEventManager gameplayEventManager;


    
    [SerializeField]
    private AIController AIController;


    void Start()
    {
        // testScript.Initialize();
        productionQueueUI?.Instantiate();
        globalVisionManager?.Initialize();
        hexTilemapManager?.Initialize();

        UIManager?.Initialize();
        buildingManager?.Instantiate();

        cityManager?.Instantiate();
        toggleManager?.Initialize();
        inputManager?.Initialize();
        gameplayCanvasManager?.Initialize();
        //grid units and kingdoms should initialize after hexTilemapManager
        foreach (BaseKingdom kingdom in kingdoms)
        {
            kingdom.Initialize();
        }
    
        selectionManager?.Instantiate();
        cityUI?.Instantiate();
        kingdomUI?.Initialize();
        addVisibleTiles?.Initialize();
        unitSpawner?.Instantiate();
        //GamePlayEventManager must be initialize first of TurnManager
        gameplayEventManager?.Initialize();
        turnManager?.Initialize();

        
        AIController?.Initialize();
    }
}
