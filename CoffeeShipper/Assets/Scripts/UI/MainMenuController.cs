using System;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private LevelConfig levelConfig;

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
    private int currentLevelIndex;

    private void Awake()
    {
        currentLevelIndex = 0;
        InitializeState(ScreenState.MainMenuScreen);
    }

    private void InitializeState(ScreenState state)
    {
        currentState = state;
        
        mainMenuScreen.SetActive(state == ScreenState.MainMenuScreen);
        pauseScreen.SetActive(state == ScreenState.PauseScreen);
        scoreScreen.SetActive(state == ScreenState.ScoreScreen);

        RemoveListeners();
        eventSystem.SetSelectedGameObject(null);
        
        switch (state)
        {
            case ScreenState.MainMenuScreen:
                startButton.onClick.AddListener(OnStartButtonClicked);
                quitButton.onClick.AddListener(OnQuitButtonClicked);
                eventSystem.SetSelectedGameObject(startButton.gameObject);
                break;
            case ScreenState.PauseScreen:
                resumeButton.onClick.AddListener(OnResumeButtonClicked);
                restartButton.onClick.AddListener(OnRestartButtonClicked);
                backToMenuButton.onClick.AddListener(OnQuitButtonClicked);
                eventSystem.SetSelectedGameObject(resumeButton.gameObject);
                break;
            case ScreenState.ScoreScreen:
                nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
                eventSystem.SetSelectedGameObject(nextLevelButton.gameObject);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            InitializeState(ScreenState.ScoreScreen);
            Show();
        }
        
        if (currentState == ScreenState.PauseScreen && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Hide();
            else
                Show();
        }
    }

    private void Show(Action onCompleteCallback = null)
    {
        isPaused = true;
        enterExitWidget.Show(onCompleteCallback);
        
        eventSystem.SetSelectedGameObject(resumeButton.gameObject);
    }

    private void Hide(Action onCompleteCallback = null)
    {
        isPaused = false;
        enterExitWidget.Hide(onCompleteCallback);
    }

    private void RemoveListeners()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        backToMenuButton.onClick.AddListener(OnQuitButtonClicked);
        
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}