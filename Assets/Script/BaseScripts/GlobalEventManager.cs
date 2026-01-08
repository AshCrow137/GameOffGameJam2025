
using UnityEngine;
using UnityEngine.Events;

    public static class GlobalEventManager
    {
        //This class used to all public UnityEvents. You can place here events and they Invoke methods 
        public static UnityEvent<float> TestUnityEvent { get; private set; } = new UnityEvent<float>();
        public static UnityEvent<HexTile, Vector3Int> OnTileClickEvent { get; private set; } = new UnityEvent<HexTile, Vector3Int>();
    public static UnityEvent<BaseKingdom> EndTurnEvent { get; private set; } = new UnityEvent<BaseKingdom>();
    public static UnityEvent<BaseKingdom> StartTurnEvent { get; private set; } = new UnityEvent<BaseKingdom> { };
    public static UnityEvent<Vector3> MouseClickedEvent { get; private set; } = new UnityEvent<Vector3>();
    //public static UnityEvent<string> ShowUIMessageEvent { get; private set; } = new UnityEvent<string> ();
    public static UnityEvent<BaseKingdom> KingdomDefeatEvent { get; private set; } = new UnityEvent<BaseKingdom>();
    public static void InvokeKingdomDefeat(BaseKingdom kingdom)
    {
        KingdomDefeatEvent.Invoke(kingdom);
    }
    //public static void InvokeShowUIMessageEvent(string message)
    //{
    //    ShowUIMessageEvent.Invoke(message);
    //}
    public static void InvokeMouseClickedEvent(Vector3 clickedPos)
    {
        MouseClickedEvent.Invoke(clickedPos);

    }
        public static void InvokeEndTurnEvent(BaseKingdom entity)
        {
            EndTurnEvent.Invoke(entity);
        } 
    public static void InvokeStartTurnEvent(BaseKingdom entity)
        {
            StartTurnEvent.Invoke(entity);
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
