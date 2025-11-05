using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    private bool PanelOpenSounds = false;
    private bool PanelOpenSettings =false;
    [SerializeField] private GameObject PanelSounds;
    [SerializeField] private GameObject PanelSettings;
    [SerializeField] private GameObject[] Buttons;
    public void Initialize()
    {
        PanelSounds.SetActive(false);
    }
    void Start()
    {
        Initialize();
    }
    public void btn_StartGame(string Scene)
    {
        SceneManager.LoadSceneAsync(Scene);
    }
    public void btn_SwitchSounds()
    {
        PanelOpenSounds = !PanelOpenSounds;
        PanelSounds.SetActive(PanelOpenSounds);
    }
    public void btn_SwitchSettings()
    {
        PanelOpenSettings = !PanelOpenSettings;
        PanelSettings.SetActive(PanelOpenSettings);
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
        button.Focus();
    }
}
