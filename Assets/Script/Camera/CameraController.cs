using UnityEngine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this value to change camera movement speed
    [SerializeField] private InputActionAsset CustomInput;
     private InputAction moveAction;
    private void OnEnable()
    {
        // Assuming you have an action map called "Player" and an action called "Move" in your asset
        var map = CustomInput.FindActionMap("Player");
        moveAction = map.FindAction("Move");
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }
    void Update()
    {

        Vector2 movementInput = moveAction.ReadValue<Vector2>();

        // Convert to Vector3 for movement on the x and y axis (or z if desired)
        Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f);

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
