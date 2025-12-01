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
    public void Initialize()
    {
        PanelSounds.SetActive(false);
        PanelSettings.SetActive(false);
        //Buttons[0].Select();
        PanelLevels.SetActive(PanelOpenLevels);
        PanelMain.SetActive(!PanelOpenLevels);
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
        AudioManager.Instance.ui_menumain_start.Post(gameObject);
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
        PanelOpenSettings = !PanelOpenSettings;
        PanelSettings.SetActive(PanelOpenSettings);
        AudioManager.Instance.ui_menumain_settings.Post(gameObject);

    }
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
