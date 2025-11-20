using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    private bool PanelOpenSounds = false;
    private bool PanelOpenSettings = false;
    [SerializeField] private InputActionAsset CustomInput;
    private InputAction moveAction;
    [SerializeField] private GameObject PanelSounds;
    [SerializeField] private GameObject PanelSettings;
    [SerializeField] private Button[] Buttons;
    private int selectBut;
    public void Initialize()
    {
        selectBut = 0;
        PanelSounds.SetActive(false);
        PanelSettings.SetActive(false);
        Buttons[0].Select();
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
    public void btn_StartGame(string Scene)
    {
        SceneManager.LoadSceneAsync(Scene);
        AudioManager.Instance.ui_menumain_start.Post(gameObject);
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
        AudioManager.Instance.ui_menumain_ambienceMusic.Post(gameObject);
    }
    public void TESTPlaySound()
    {
        AudioManager.Instance.ui_menumain_settings.Post(gameObject);
    }
    public void btn_QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
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
