using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainMenuController
{
    private void OnNextLevelButtonClicked()
    {
        int nextLevelIndex = levelConfig.GetNextLevelIndex(currentLevelIndex);
        if (nextLevelIndex == -1)
        {
            // For simplicity on last level, open screen with Resume, Restart, Quit buttons instead.
            InitializeState(ScreenState.PauseScreen);
            return;
        }

        SceneManager.LoadScene(levelConfig.Levels[nextLevelIndex].Scene, LoadSceneMode.Single);
        currentLevelIndex = nextLevelIndex;

        Hide(() =>
        {
            currentCoffeesDelivered = 0;
            TotalMachineTrips = 0;
            TotalCoffeesLost = 0;
            
            InitializeState(ScreenState.PauseScreen);
            UnsusbscribeActions();
            SusbscribeActions();
        });
    }
    
    private void OnStartButtonClicked()
    {
        enterExitWidget.HideFirstTime(() =>
        {
            isPaused = false;
            SetPausablesPaused(false);
            InitializeState(ScreenState.PauseScreen);
            SusbscribeActions();
        });
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        return;
#endif
        Application.Quit();
    }

    private void OnResumeButtonClicked()
    {
        Hide();
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Hide();
    }
}
