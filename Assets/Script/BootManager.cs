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
    private CityUI cityUI;
    [SerializeField]
    private KingdomUI kingdomUI;
    [SerializeField]
    private AddVisibleTiles addVisibleTiles;

    void Start()
    {
        // testScript.Initialize();
        hexTilemapManager?.Initialize();

        turnManager?.Initialize();
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
    }
}
