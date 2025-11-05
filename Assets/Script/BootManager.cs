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
        private BaseGridUnitScript[] units;
        [SerializeField]
        private BuildingManager buildingManager;

    


    void Start()
    {
            // testScript.Initialize();
        hexTilemapManager?.Initialize();
        
        turnManager?.Initialize();
        //grid units should initialize afte hexTilemapManager
            foreach (var unit in units)
        {
            unit?.Initialize();
        }
        
        buildingManager?.Instantiate();
        AstarPath.active.Scan();
    }
}
