using UnityEngine;


    public class BootManager : MonoBehaviour
    {
        //In this example, you have to put value in testScript directly from Inspector in Unity
        [SerializeField] 
        private TestScript testScript;
        [SerializeField]
        private HexTilemapManager hexTilemapManager;
    [SerializeField]
    private TurnManager turnManager;


    void Start()
        [SerializeField]
        private BaseGridUnitScript unit;
        
        
        void Start()
        {
            // testScript.Initialize();
            hexTilemapManager?.Initialize();
        turnManager?.Initialize();
            hexTilemapManager.Initialize();
        print("scan");
            AstarPath.active.Scan();
        print("scanFinished");
            unit.Initialize();
        }
    }
