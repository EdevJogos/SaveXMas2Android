using UnityEngine;

public class GameCEO : MonoBehaviour
{
    public static event System.Action onGameStateChanged;

    public static bool Tutorial { get; private set; } = true;
    public static GameState State { get; private set; }

    public GUIManager guiManager;
    public InputManager inputManager;
    public PlayerManager playerManager;
    public StageManager stageManager;
    public CameraManager cameraManager;
    public ScoreManager scoreManager;
    public AudioManager audioManager;
    public AdsManager adsManager;
    public GPServicesManager GPServicesManager;

    public ParticlesDabatase particlesDabatase;

    private void Awake()
    {
        ChangeGameState(GameState.LOADING);

        LanguageManager.onLanguageUpdated += LanguageManager_onLanguageUpdated;

        guiManager.onStartGameRequested += GuiManager_onStartGameRequested;
        guiManager.onShopRequested += GuiManager_onShopRequested;
        guiManager.onHireRequested += GuiManager_onHireRequested;
        guiManager.onExitGameRequested += GuiManager_onExitGameRequested;
        guiManager.onResumePlayRequested += GuiManager_onResumePlayRequested;
        guiManager.onCreditsRequested += GuiManager_onCreditsRequested;
        guiManager.onHomeRequested += GuiManager_onHomeRequested;
        guiManager.onTutorialCompleted += GuiManager_onTutorialCompleted;

        stageManager.onHireTimerUpdated += StageManager_onHireTimerUpdated;
        stageManager.onScoreGift += StageManager_onScoreGift;
        stageManager.onGiftLost += StageManager_onGiftLost;
        stageManager.onGameOver += StageManager_onGameOver;

        adsManager.onHireYetiRequested += AdsManager_onHireYetiRequested;

        particlesDabatase.Initiate();
        audioManager.Initate();
        scoreManager.Initiate();
        cameraManager.Initiate();
        guiManager.Initiate();
        stageManager.Initiate();
        playerManager.Initiate();
    }

    private void Start()
    {
        audioManager.Initialize();
        guiManager.Initialize();

        guiManager.ShowDisplay(Displays.LANGUAGE);
        ChangeGameState(GameState.INTRO);
    }

    //-----------------CEO------------------

    private void LanguageManager_onLanguageUpdated()
    {
        guiManager.ShowDisplay(Displays.INTRO);
    }

    private void StartGame()
    {
        scoreManager.ResetScore();
        stageManager.ClearGifts();
        stageManager.InitializeStage();
        playerManager.InitializePlayer();

        guiManager.UpdateDisplay(Displays.HUD, 0, scoreManager.currentScore, stageManager.streak);
        guiManager.UpdateDisplay(Displays.HUD, 1, stageManager.lostGifts);

        guiManager.ShowDisplay(Displays.HUD);

        ChangeGameState(GameState.PLAY);
    }

    private void ResetGame()
    {
        playerManager.ResetUnits();
        stageManager.ClearGifts();
    }

    private void ChangeGameState(GameState p_state)
    {
        State = p_state;

        onGameStateChanged?.Invoke();
    }

    //-----------------GUI MANAGER------------------

    private void GuiManager_onStartGameRequested()
    {
        if(Tutorial)
        {
            Tutorial = false;

            guiManager.ShowDisplay(Displays.TUTORIAL);
            ChangeGameState(GameState.TUTORIAL);
        }
        else
        {
            StartGame();
        }
    }

    private void GuiManager_onShopRequested()
    {
        ChangeGameState(GameState.PAUSE);

        guiManager.ShowDisplay(Displays.SHOP);
    }

    private void GuiManager_onHireRequested()
    {
        adsManager.ShowRewardedVideo();
    }

    private void GuiManager_onExitGameRequested()
    {
        ResetGame();

        guiManager.ShowDisplay(Displays.INTRO);

        ChangeGameState(GameState.INTRO);
    }

    private void GuiManager_onResumePlayRequested()
    {
        guiManager.ShowDisplay(Displays.HUD);

        ChangeGameState(GameState.PLAY);
    }

    private void GuiManager_onCreditsRequested()
    {
        guiManager.ShowDisplay(Displays.CREDITS);
    }

    private void GuiManager_onHomeRequested()
    {
        guiManager.ShowDisplay(Displays.INTRO);
    }

    private void GuiManager_onTutorialCompleted()
    {
        StartGame();
    }

    //-----------------SCORE MANAGER----------------

    //-----------------STAGE MANAGER----------------

    private void StageManager_onHireTimerUpdated()
    {
        if (playerManager.TotalYetis < GameConfig.MAX_YETIS)
        {
            guiManager.UpdateDisplay(Displays.SHOP, 1, 0);
        } 
    }

    private void StageManager_onScoreGift(Gift p_gift, Vector2 p_position)
    {
        scoreManager.ScorePoints(p_gift, stageManager.streak, p_position);
        guiManager.UpdateDisplay(Displays.HUD, 0, scoreManager.currentScore, stageManager.streak);
    }

    private void StageManager_onGiftLost()
    {
        guiManager.UpdateDisplay(Displays.HUD, 0, scoreManager.currentScore, stageManager.streak);
        guiManager.UpdateDisplay(Displays.HUD, 1, stageManager.lostGifts);
    }

    private void StageManager_onGameOver()
    {
        ChangeGameState(GameState.GAME_OVER);

        GPServicesManager.PostScore(scoreManager.currentScore);

        guiManager.UpdateDisplay(Displays.GAME_OVER, 0, new int[3] { scoreManager.currentScore, stageManager.caughtGifts, stageManager.bestStreak });
        guiManager.ShowDisplay(Displays.GAME_OVER);
    }

    private void AdsManager_onHireYetiRequested()
    {
        playerManager.HireYeti();

        if (playerManager.TotalYetis < GameConfig.MAX_YETIS)
        {
            stageManager.ResetHireTimer();
        }
        else
        {
            guiManager.UpdateDisplay(Displays.SHOP, 0, 0);
        }
    }
}
