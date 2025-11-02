
    using UnityEngine.Events;

    public static class GlobalEventManager
    {
        public static UnityEvent TestUnityEvent { get; private set; } = new UnityEvent();

        public static void InvokeTestUnityEvent(float someVariable)
        {
            TestUnityEvent.Invoke();
        }
    }
