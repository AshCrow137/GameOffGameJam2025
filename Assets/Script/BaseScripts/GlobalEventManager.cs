
using UnityEngine;
using UnityEngine.Events;

    public static class GlobalEventManager
    {
        //This class used to all public UnityEvents. You can place here events and they Invoke methods 
        public static UnityEvent<float> TestUnityEvent { get; private set; } = new UnityEvent<float>();
        public static UnityEvent<HexTile, Vector3Int> OnTileClickEvent { get; private set; } = new UnityEvent<HexTile, Vector3Int>();
    public static UnityEvent<GameObject> EndTurnEvent { get; private set; } = new UnityEvent<GameObject>();
        public static void InvokeEndTurnEvent(GameObject entity)
        {
            EndTurnEvent.Invoke(entity);
        }
        public static void InvokeTestUnityEvent(float someVariable)
        {
            TestUnityEvent.Invoke(someVariable);
        }

    public static void InvokeOnTileClickEvent(HexTile clickedTile,Vector3Int cellPos)
    {
        OnTileClickEvent.Invoke(clickedTile, cellPos);
    }
    }
