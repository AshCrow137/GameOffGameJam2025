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
    private BaseGridUnitScript[] units;

    [SerializeField]
    private BaseKingdom unitOwner;



    void Start()
    {
            // testScript.Initialize();
        hexTilemapManager?.Initialize();
        
        turnManager?.Initialize();
        //grid units should initialize afte hexTilemapManager
        foreach (var unit in units)
        {
            unit.Initialize(unitOwner);
        }
        
        buildingManager?.Instantiate();
        AstarPath.active.Scan();
        cityManager?.Instantiate();
    }
}
