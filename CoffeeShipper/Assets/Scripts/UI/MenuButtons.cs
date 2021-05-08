using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainMenuController
{
    private void OnNextLevelButtonClicked()
    {
        // If we're on the last level, "Next Level" just restarts the same last level.
        if (currentLevelIndex >= levelConfig.Levels.Length - 1)
            currentLevelIndex = levelConfig.Levels.Length - 2;
                
        SceneManager.LoadScene(levelConfig.Levels[currentLevelIndex++], LoadSceneMode.Single);
        Hide(() => InitializeState(ScreenState.PauseScreen));
    }
    
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene(levelConfig.Levels[0], LoadSceneMode.Single);
        Hide(() => InitializeState(ScreenState.PauseScreen));
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

// TODO
// Buttons small anim?
// Stats on score screen