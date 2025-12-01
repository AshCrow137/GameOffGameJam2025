using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
public class MainMenuController : MonoBehaviour
{
    private bool PanelOpenSounds = false;
    private bool PanelOpenSettings = false;
    private bool PanelOpenLevels = false;
    [SerializeField] private InputActionAsset CustomInput;
    private InputAction moveAction;
    [SerializeField] private GameObject PanelSounds;
    [SerializeField] private GameObject PanelSettings;
    [SerializeField] private GameObject PanelLevels;
    [SerializeField] private GameObject PanelMain;
    [SerializeField] private Button[] Buttons;
    [SerializeField] private string[] LevelsName;
    private string currentLevel="";
    [SerializeField] private Button ButtonStartLevel;
    [SerializeField]
    private Slider cameraRotationSlider; 
    [SerializeField]
    private Slider cameraSpeedSlider;  
    [SerializeField]
    private Slider cameraSpeedEdgeScreenSlider;


    private float CameraRotationSpeed = 1;
    private float CameraSpeed = 1;
    private float CameraSpeedEdgeScreen = 1;
    public void Initialize()
    {
        PanelSounds.SetActive(false);
        PanelSettings.SetActive(false);
        //Buttons[0].Select();
        PanelLevels.SetActive(PanelOpenLevels);
        PanelMain.SetActive(!PanelOpenLevels);
        CameraRotationSpeed = PlayerPrefs.HasKey("CameraRotationSpeed") ? PlayerPrefs.GetFloat("CameraRotationSpeed") : CameraRotationSpeed;
        CameraSpeedEdgeScreen = PlayerPrefs.HasKey("CameraSpeedEdgeScreen") ? PlayerPrefs.GetFloat("CameraSpeedEdgeScreen") : CameraSpeedEdgeScreen;
        CameraSpeed = PlayerPrefs.HasKey("CameraSpeed") ? PlayerPrefs.GetFloat("CameraSpeed") : CameraSpeed;
        cameraRotationSlider.value = CameraRotationSpeed;
        cameraSpeedSlider.value = CameraSpeed;
        cameraSpeedEdgeScreenSlider.value = CameraSpeedEdgeScreen;
    }
    void Start()
    {
        Initialize();
        
    }
    private void OnEnable()
    {
        var map = CustomInput.FindActionMap("InMenu");
        moveAction = map.FindAction("Navigate");
        moveAction.Enable();
        
    }
    private void OnDisable() {
        moveAction.Disable(); 
        }
    public void btn_StartGame()
    {
        AudioManager.Instance.ui_menumain_start.Post(gameObject);
        SceneManager.LoadSceneAsync(currentLevel);
    }
    public void tgl_Level(int level)
    {
        currentLevel=LevelsName[level];
        ButtonStartLevel.interactable=true;
    }
    public void btn_OpenLevels()
    {
        //SceneManager.LoadSceneAsync(Scene);
        AudioManager.Instance.ui_menumain_continue.Post(gameObject);
        currentLevel="";
        ButtonStartLevel.interactable=false;
        PanelOpenLevels = !PanelOpenLevels;
        PanelLevels.SetActive(PanelOpenLevels);
        PanelMain.SetActive(!PanelOpenLevels);
    }
    public void btn_SwitchSounds()
    {
        PanelOpenSounds = !PanelOpenSounds;
        PanelSounds.SetActive(PanelOpenSounds);
        AudioManager.Instance.ui_menumain_volume.Post(gameObject);
    }
    public void btn_SwitchSettings()
    {
        //PanelOpenSettings = !PanelOpenSettings;
        //PanelSettings.SetActive(PanelOpenSettings);
        AudioManager.Instance.ui_menumain_settings.Post(gameObject);

    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("CameraRotationSpeed", CameraRotationSpeed);
        PlayerPrefs.SetFloat("CameraSpeedEdgeScreen", CameraSpeedEdgeScreen);
        PlayerPrefs.SetFloat("CameraSpeed", CameraSpeed);
    }

    public void sldr_SetRotateSpeed(Slider sld) => CameraRotationSpeed = sld.value;
    public void sldr_SetCameraSpeedEdgeScreen(Slider sld) => CameraSpeedEdgeScreen = sld.value;
    public void sldr_SetCameraSpeed(Slider sld) => CameraSpeed = sld.value;
    public void TESTPlaySound()
    {
        AudioManager.Instance.ui_menumain_settings.Post(gameObject);
    }
    public void btn_QuitGame()
    {
        Application.Quit();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
    }
    void Update()
    {

    }
    // private void OnNavigateStart(InputAction.CallbackContext context)
    // {
    //     // selectBut += (int)context.ReadValue<Vector2>().y;
    //     // if (selectBut < 0)
    //     //     selectBut = 0;
    //     // else if (selectBut >= Buttons.Length)
    //     //     selectBut = Buttons.Length - 1;

    //     // Buttons[selectBut].Select();  
    //     // Debug.Log(selectBut);

    // }
}
