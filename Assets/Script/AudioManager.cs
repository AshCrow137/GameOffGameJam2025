using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enums to classify states
public enum WwiseGameState { GamePaused, GameNone };
public enum WwiseMusicState { MusicMainMenu, MusicIntro, MusicPathHigher, MusicPathLower, MusicChase, None };

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    private bool bIsInitialized = false;

    [Header("Soundbanks")]
    [SerializeField] private List<AK.Wwise.Bank> Soundbanks;

    [Header("Game State Variables")]
    [SerializeField] private AK.Wwise.State Game_Paused;
    [SerializeField] private AK.Wwise.State Game_None;

    private WwiseGameState currentGameState;

    //[Header("MainMenu")]
    [SerializeField] private AK.Wwise.State Music_MusicMainMenu;
    [SerializeField] private AK.Wwise.State Music_MusicIntro;
    [SerializeField] private AK.Wwise.State Music_MusicPathHigher;
    [SerializeField] private AK.Wwise.State Music_MusicPathLower;
    [SerializeField] private AK.Wwise.State Music_MusicChase;
    [SerializeField] private AK.Wwise.State Music_None;

    //[Header("Music State Variables")]
    [SerializeField] private AK.Wwise.State Music_MusicMainMenu;
    [SerializeField] private AK.Wwise.State Music_MusicIntro;
    [SerializeField] private AK.Wwise.State Music_MusicPathHigher;
    [SerializeField] private AK.Wwise.State Music_MusicPathLower;
    [SerializeField] private AK.Wwise.State Music_MusicChase;
    [SerializeField] private AK.Wwise.State Music_None;

    private WwiseMusicState currentMusicState;

    //[Header("Wwise Music Events")]
    [SerializeField] public AK.Wwise.Event Play_Music;
    [SerializeField] public AK.Wwise.Event Stop_Music;

    [Header("Wwise SFX Events")]

    [Header("MainMenu")]
    [SerializeField] public AK.Wwise.Event ui_menumain_credits;
    [SerializeField] public AK.Wwise.Event ui_menumain_continue;
    [SerializeField] public AK.Wwise.Event ui_menumain_exit;
    [SerializeField] public AK.Wwise.Event ui_menumain_mainmenu;
    [SerializeField] public AK.Wwise.Event ui_menumain_settings;
    [SerializeField] public AK.Wwise.Event ui_menumain_start;
    [SerializeField] public AK.Wwise.Event ui_menumain_volume;

    //[Header("SFX Dialogue")]
    [SerializeField] public AK.Wwise.Event E_vox_empathy;
    [SerializeField] public AK.Wwise.Event E_vox_logic;
    [SerializeField] public AK.Wwise.Event E_vox_watchdog;

    //[Header("SFX Player")]

    [SerializeField] public AK.Wwise.Event E_player_climb;
    [SerializeField] public AK.Wwise.Event E_player_damage;
    [SerializeField] public AK.Wwise.Event E_player_death;
    [SerializeField] public AK.Wwise.Event E_player_footsteps;
    [SerializeField] public AK.Wwise.Event E_player_sliding;

    [Header("SFX UI InGame")]

    [Header("SFX UI Menu")]

    [SerializeField] public AK.Wwise.Event E_mainmenu_start;
    [SerializeField] public AK.Wwise.Event E_mainmenu_toggle;

    [Header("SFX World")]

    [SerializeField] public AK.Wwise.Event E_play_elevator;
    [SerializeField] public AK.Wwise.Event E_secret;
    [SerializeField] public AK.Wwise.Event E_stop_elevator;
    [SerializeField] public AK.Wwise.Event E_switch_interact;

    private void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWwiseGameState(WwiseGameState.GameNone);
        SetWwiseMusicState(WwiseMusicState.MusicMainMenu);

        //PlayMusic.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Initialize()
    {
        //Singleton logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("AudioManager already exists! Destroying new instance.");
            Destroy(this);
        }


        if (!bIsInitialized)
        {
            LoadSoundbanks();
        }
        //initaliases no GameState/MusicState set
        SetWwiseGameState(WwiseGameState.GameNone);
        SetWwiseMusicState(WwiseMusicState.None);

        bIsInitialized = true;


    }

    void LoadSoundbanks()
    {
        if (Soundbanks.Count > 0)
        {
            foreach (AK.Wwise.Bank bank in Soundbanks)
                bank.Load();

            Debug.Log("Starup Soundbanks have been loaded");
        }

        else
        {
            Debug.LogError("Soundbanks list is empty! Are the banks assigned to the AudioManager?");
        }

    }
    public void SetWwiseGameState(WwiseGameState GameState)
    {
        // checks if same as a value we store 
        if (GameState == currentGameState)
        {
            Debug.Log("GameState is already" + GameState + ".");
            return;
        }

        switch (GameState)
        {
            case (WwiseGameState.GamePaused):
                Game_Paused.SetValue();
                break;
            case (WwiseGameState.GameNone):
                Game_None.SetValue();
                break;

        }

        Debug.Log("New Wwise GameState: " + GameState + ".");

        currentGameState = GameState;
    }

    public void SetWwiseMusicState(WwiseMusicState MusicState)
    {
        if (MusicState == currentMusicState)
        {
            Debug.Log("MusicState is already" + MusicState + ".");
            return;
        }

        switch (MusicState)
        {
            case (WwiseMusicState.MusicMainMenu):
                Music_MusicMainMenu.SetValue();
                break;
            case (WwiseMusicState.MusicIntro):
                Music_MusicIntro.SetValue();
                break;
            case (WwiseMusicState.MusicPathHigher):
                Music_MusicPathHigher.SetValue();
                break;
            case (WwiseMusicState.MusicPathLower):
                Music_MusicPathLower.SetValue();
                break;
            case (WwiseMusicState.MusicChase):
                Music_MusicChase.SetValue();
                break;

                Debug.Log("New Wwise MusicState: " + MusicState + ".");

                currentMusicState = MusicState;

        }

    }

}
