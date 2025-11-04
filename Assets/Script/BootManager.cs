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
        private BaseGridUnitScript unit;
        [SerializeField]
        private BuildingManager buildingManager;

    


    void Start()
    {
            // testScript.Initialize();
        hexTilemapManager?.Initialize();
        AstarPath.active.Scan();
        turnManager?.Initialize();
            
        unit?.Initialize();
        buildingManager?.Instantiate();
    }
}
