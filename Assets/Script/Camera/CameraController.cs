using UnityEngine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{
    
    [SerializeField] private InputActionAsset CustomInput;
    private InputAction moveAction;
    private InputAction moveActionMouse;
    private const float EDGE_THRESHOLD = 0.4f;//a variable indicating how far the camera will move to the end of the screen.
    public float moveSpeed = 5f;//Speed Camera
    private void OnEnable()
    {
        var map = CustomInput.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        moveActionMouse = map.FindAction("MousePos");
        moveAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
    }
    void Update()
    {
        Vector2 mousepos = moveActionMouse.ReadValue<Vector2>();
        Vector2 screenUV = new Vector2(mousepos.x / Screen.width - .5f, mousepos.y / Screen.height - .5f);
        Vector3 move = Vector3.zero;
        if (screenUV.x < -EDGE_THRESHOLD) move.x = -1f;
        if (screenUV.x > EDGE_THRESHOLD)  move.x =  1f;
        if (screenUV.y < -EDGE_THRESHOLD) move.y = -1f;
        if (screenUV.y > EDGE_THRESHOLD)  move.y =  1f;
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        
        

        Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);
        transform.Translate(Time.deltaTime * (move+movement) * moveSpeed);
    }
}
