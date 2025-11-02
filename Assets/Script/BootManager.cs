using UnityEngine;


    public class BootManager : MonoBehaviour
    {
        //In this example, you have to put value in testScript directly from Inspector in Unity
        [SerializeField] 
        private TestScript testScript;
        
        
        void Start()
        {
            testScript.Initialize();
        }
    }
