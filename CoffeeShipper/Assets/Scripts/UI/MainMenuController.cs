using UnityEngine;

using UnityEngine.UI;

public partial class MainMenuController : MonoBehaviour
{
    private enum ScreenState
    {
        MainMenuScreen,
        PauseScreen,
        ScoreScreen
    }

    #region Reference Fields

    [SerializeField]
    private EnterExitWidget enterExitWidget;

    [Header("Main Menu")]
    [SerializeField]
    private GameObject mainMenuScreen;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button quitButton;

    [Header("Pause Screen")]
    [SerializeField]
    private GameObject pauseScreen;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button backToMenuButton;

    [Header("Score Screen")]
    [SerializeField]
    private GameObject scoreScreen;

    [SerializeField]
    private Button nextLevelButton;

    #endregion

    private bool isPaused = false;
    private ScreenState currentState = ScreenState.MainMenuScreen;

    private void Awake()
    {
        InitializeState(ScreenState.MainMenuScreen);
    }

    private void InitializeState(ScreenState state)
    {
        currentState = state;
        
        mainMenuScreen.SetActive(state == ScreenState.MainMenuScreen);
        pauseScreen.SetActive(state == ScreenState.PauseScreen);
        scoreScreen.SetActive(state == ScreenState.ScoreScreen);
        
        switch (state)
        {
            case ScreenState.MainMenuScreen:
                startButton.onClick.AddListener(OnStartButtonClicked);
                quitButton.onClick.AddListener(OnQuitButtonClicked);
                break;
            case ScreenState.PauseScreen:
                // resumeButton.onClick.AddListener();
                // levelSelectButton.onClick.AddListener();
                // backToMenuButton.onClick.AddListener();
                break;
            case ScreenState.ScoreScreen:
                // nextLevelButton.onClick.AddListener();
                break;
        }
    }

    private void Update()
    {
        if (currentState == ScreenState.PauseScreen && Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            UpdateShow();
        }
    }

    private void UpdateShow()
    {
        if (isPaused)
            enterExitWidget.Hide();
        else
            enterExitWidget.Show();
    }

    private void Show()
    {
        enterExitWidget.Show();
    }

    private void Hide()
    {
        enterExitWidget.Hide();
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }
}