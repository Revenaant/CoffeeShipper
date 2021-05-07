using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MainMenuController
{
    private void OnNextLevelButtonClicked()
    {
        // TODO get next scene
    }
    
    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("level 0", LoadSceneMode.Single);
        InitializeState(ScreenState.PauseScreen);
        
        isPaused = false;
        UpdateShow();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    private void OnResumeButtonClicked()
    {
        isPaused = false;
        UpdateShow();
    }

    private void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        isPaused = false;
        UpdateShow();
    }
    
    private void OnBackToMenuButtonClicked()
    {
        InitializeState(ScreenState.MainMenuScreen);
    }
}

// TODO
// Buttons work with keyboard
// Buttons small anim?
// Test states opening
// Stats on score screen
// 