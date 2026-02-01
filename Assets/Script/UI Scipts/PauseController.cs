using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    [SerializeField] private GameObject PausePanel;
    private bool isPaused;
    [SerializeField] private InputActionAsset CustomInput;
    private InputAction moveAction;
    public void Initialize()
    {
        isPaused = false;
        PausePanel.SetActive(isPaused);
        Time.timeScale = 1f;
    }
    void Start() => Initialize();
    private void OnEnable()
    {
        var map = CustomInput.FindActionMap("InGame");
        moveAction = map.FindAction("PauseGame");
        moveAction.performed += CloseOpenPuaseWithBind;
        moveAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.performed -= CloseOpenPuaseWithBind;
        moveAction.Disable();
    }
    public void btn_QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void CloseOpenPuaseWithBind(InputAction.CallbackContext ctx) => CloseOpenPuase();
    public void CloseOpenPuase()
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
