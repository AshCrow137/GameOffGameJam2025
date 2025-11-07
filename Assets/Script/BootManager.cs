using UnityEngine;


public class BootManager : MonoBehaviour
{
        ////In this example, you have to put value in testScript directly from Inspector in Unity
        //[SerializeField] 
        //private TestScript testScript;
        [SerializeField]
        private HexTilemapManager hexTilemapManager;
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



    void Start()
    {
            // testScript.Initialize();
        hexTilemapManager?.Initialize();
        
        turnManager?.Initialize();
        //grid units should initialize afte hexTilemapManager
        foreach (BaseKingdom kingdom in kingdoms)
        {
            kingdom.Initialize();
        }
        
        buildingManager?.Instantiate();
        AstarPath.active.Scan();
        cityManager?.Instantiate();
        toggleManager?.Initialize();
    }
}
