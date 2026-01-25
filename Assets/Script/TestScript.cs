using UnityEngine;


public class TestScript : MonoBehaviour
{
    public void Initialize()
    {
        GlobalEventManager.TestUnityEvent.AddListener(OnTestEvent);
        GlobalEventManager.InvokeTestUnityEvent(someVariable: 15);

    }



    private void OnTestEvent(float value)
    {
        //do something
    }
}
