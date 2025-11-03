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
        [SerializeField]
        private BaseGridUnitScript unit;
        
        
        void Start()
        {
            // testScript.Initialize();
            hexTilemapManager?.Initialize();
            turnManager?.Initialize();
            print("scan");
            AstarPath.active?.Scan();
            print("scanFinished");
            unit?.Initialize();
        }

    bool b = true;
    float t = 5;
    private void Update()
    {
        if(b&&t<=0)
        {
            AstarPath.active?.Scan();
            
            b = false;
        }else
        {
            t -= Time.deltaTime;
        }
           
    }
}
