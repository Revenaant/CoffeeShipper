using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    private TextMeshProUGUI deliveredText;

    [SerializeField]
    private TextMeshProUGUI lostText;

    [SerializeField]
    private TextMeshProUGUI tripsText;

    #endregion

    private bool isPaused = false;
    private ScreenState currentState = ScreenState.MainMenuScreen;

    private int currentLevelIndex;
    private int currentCoffeesDelivered;

    // Stats
    private int TotalCoffeesLost = 0;
    private int TotalMachineTrips = 0;

    private void Awake()
    {
        currentLevelIndex = 0;
        InitializeState(ScreenState.MainMenuScreen);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Call this once going from Bootstrap to 1st level so player starts Paused (Until we click start)
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        StartCoroutine(DelayedCallbackEnumerator(0.1f, () => SetPausablesPaused(true)));
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
        SetPausablesPaused(true);

        enterExitWidget.Show(onCompleteCallback);
        eventSystem.SetSelectedGameObject(resumeButton.gameObject);
    }

    private void Hide(Action onCompleteCallback = null)
    {
        isPaused = false;
        SetPausablesPaused(false);
        enterExitWidget.Hide(onCompleteCallback);
    }

    private void SetPausablesPaused(bool paused)
    {
        IEnumerable<IPause> pausables = FindInterfacesOfType<IPause>();
        foreach (IPause pausable in pausables)
        {
            if (paused)
                pausable.Pause();
            else
                pausable.Unpause();
        }
    }

    private static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
    {
        foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            foreach (T child in go.GetComponentsInChildren<T>(includeInactive))
                yield return child;
        }
    }

    private void RemoveListeners()
    {
        startButton.onClick.RemoveListener(OnStartButtonClicked);
        quitButton.onClick.RemoveListener(OnQuitButtonClicked);

        resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        backToMenuButton.onClick.RemoveListener(OnQuitButtonClicked);

        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
    }

    private void OnDestroy()
    {
        RemoveListeners();
        UnsusbscribeActions();
    }

    private void SusbscribeActions()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null)
            return;

        player.OnCoffeeLost += OnCoffeeLost;
        player.OnCoffeeDelivered += OnCoffeeDelivered;
        player.OnTripToCoffeeMachine += OnTripToMachine;
    }

    private void UnsusbscribeActions()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null)
            return;

        player.OnCoffeeLost -= OnCoffeeLost;
        player.OnCoffeeDelivered -= OnCoffeeDelivered;
        player.OnTripToCoffeeMachine -= OnTripToMachine;
    }

    private void OnCoffeeLost()
    {
        TotalCoffeesLost++;
    }

    private void OnTripToMachine()
    {
        TotalMachineTrips++;
    }

    private void OnCoffeeDelivered()
    {
        currentCoffeesDelivered++;

        if (currentCoffeesDelivered >= levelConfig.Levels[currentLevelIndex].CoffeesRequired)
        {
            InitializeState(ScreenState.ScoreScreen);

            deliveredText.text = $"Coffees Delivered: {currentCoffeesDelivered}";
            lostText.text = $"Coffees Lost: {TotalCoffeesLost - currentCoffeesDelivered}";
            tripsText.text = $"Trips to the Coffee Machine: {TotalMachineTrips}";

            // Last level
            if (currentLevelIndex >= levelConfig.Levels.Length - 1)
                nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";

            Show();
        }
    }

    private IEnumerator DelayedCallbackEnumerator(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}
