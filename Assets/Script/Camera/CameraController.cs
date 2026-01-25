using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float CameraRotationSpeed = 3;
    [SerializeField]
    private float CameraSpeed = 1;
    [SerializeField]
    private float CameraSpeedEdgeScreen = 1;
    [SerializeField]
    private Transform Camera;
    [SerializeField] private InputActionAsset CustomInput;
    private InputAction moveAction;
    private InputAction moveActionMouse;
    private InputAction moveCameraZoom;
    private InputAction rotateAction;
    [SerializeField]
    private float EDGE_THRESHOLD = 0.4f;//a variable indicating how far the camera will move to the end of the screen.

    public float moveSpeed = 5f;//Speed Camera
    [SerializeField]
    private Transform CameraArm;
    public CinemachineCamera vcam;
    private bool bCameraPosFixed = false;
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Slider cameraRotationSlider;
    [SerializeField]
    private Slider cameraSpeedSlider;
    [SerializeField]
    private Slider cameraSpeedEdgeScreenSlider;

    [SerializeField]
    private float CameraXBordedMax = 15;
    [SerializeField]
    private float CameraXBordedMin = -15;
    [SerializeField]
    private float CameraYBordedMax = 15;
    [SerializeField]
    private float CameraYBordedMin = -15;
    public Camera GetMainCamera()
    {
        return mainCamera;
    }
    public Transform GetCameraArmTransform()
    { return CameraArm; }

    public static CameraController instance { get; private set; }

    public void Initialize()
    {
        CameraRotationSpeed = PlayerPrefs.HasKey("CameraRotationSpeed") ? PlayerPrefs.GetFloat("CameraRotationSpeed") : CameraRotationSpeed;
        CameraSpeedEdgeScreen = PlayerPrefs.HasKey("CameraSpeedEdgeScreen") ? PlayerPrefs.GetFloat("CameraSpeedEdgeScreen") : CameraSpeedEdgeScreen;
        CameraSpeed = PlayerPrefs.HasKey("CameraSpeed") ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed;
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        cameraRotationSlider.value = CameraRotationSpeed;
        cameraSpeedSlider.value = CameraSpeed;
        cameraSpeedEdgeScreenSlider.value = CameraSpeedEdgeScreen;
    }
    private void OnEnable()
    {
        var map = CustomInput.FindActionMap("InGame");
        moveCameraZoom = map.FindAction("CameraZoom");
        rotateAction = map.FindAction("RotateCamera");

    }
    private void OnDisable()
    {

    }
    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("CameraRotationSpeed", CameraRotationSpeed);
        PlayerPrefs.SetFloat("CameraSpeedEdgeScreen", CameraSpeedEdgeScreen);
        PlayerPrefs.SetFloat("CameraSpeed", CameraSpeed);

    }
    public Transform GetCameraArm() { return CameraArm; }
    void LateUpdate()
    {
        if (!bCameraPosFixed)
        {
            Vector2 mousepos = InputManager.instance.GetMousePosVector2();
            Vector2 screenUV = new Vector2(mousepos.x / Screen.width - .5f, mousepos.y / Screen.height - .5f);
            float rotateValue = rotateAction.ReadValue<float>();
            Vector3 move = Vector3.zero;
            if (screenUV.x < -EDGE_THRESHOLD) move.x = -CameraSpeedEdgeScreen;
            if (screenUV.x > EDGE_THRESHOLD) move.x = CameraSpeedEdgeScreen;
            if (screenUV.y < -EDGE_THRESHOLD) move.y = -CameraSpeedEdgeScreen;
            if (screenUV.y > EDGE_THRESHOLD) move.y = CameraSpeedEdgeScreen;
            Vector2 movementInput = InputManager.instance.CameraKeyMovementDirection;
            // the final calculation of the movement vector and the movement itself
            Vector3 movement = new Vector3(movementInput.x * CameraSpeed, movementInput.y * CameraSpeed, 0f);
            transform.Translate(Time.deltaTime * (move + movement) * moveSpeed);
            transform.Rotate(Vector3.forward * rotateValue * CameraRotationSpeed);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, CameraXBordedMin, CameraXBordedMax), Mathf.Clamp(transform.position.y, CameraYBordedMin, CameraYBordedMax), transform.position.z);
            var orbital = vcam.GetComponent<CinemachineOrbitalFollow>();
            if (!UIManager.Instance.isOnCanvas) orbital.VerticalAxis.Value = Mathf.Clamp(orbital.VerticalAxis.Value + (-1) * moveCameraZoom.ReadValue<float>() * 2f, 200, 250);


        }

    }

    public void SmoothMoveCameraToPosition(Vector3 position)
    {
        bCameraPosFixed = true;
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
    }
    public void UnlockCameraMovement()
    {
        bCameraPosFixed = false;
    }
    public void sldr_SetRotateSpeed(Slider sld) => CameraRotationSpeed = sld.value;
    public void sldr_SetCameraSpeedEdgeScreen(Slider sld) => CameraSpeedEdgeScreen = sld.value;
    public void sldr_SetCameraSpeed(Slider sld) => CameraSpeed = sld.value;
}
