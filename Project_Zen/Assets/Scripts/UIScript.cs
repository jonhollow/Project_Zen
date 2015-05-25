using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that holds general UI behavior
/// </summary>
public class UIScript : MonoBehaviour
{
    #region Fields

    public GameObject loadMenu; // The menu for choosing a file to load

    const string EDITOR_SCENE = "LevelEditor";  // The name of the level editor scene
    const string GAME_SCENE = "Game";           // The name of the game scene
    const string MAIN_MENU_SCENE = "MainMenu";  // The name of the main menu scene

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the load button being pressed
    /// </summary>
    public void LoadButtonPressed()
    {
        // Opens the load menu if there are any saved levels
        if (LevelController.Instance.SavedLevels.Count > 0)
        { loadMenu.SetActive(true); }
    }

    /// <summary>
    /// Opens the level editor
    /// </summary>
    public void OpenLevelEditor()
    {
        // Sets in level editor, clears level data, and loads the level editor
        LevelController.Instance.InLevelEditor = true;
        LevelController.Instance.ClearData();
        Application.LoadLevel(EDITOR_SCENE);
    }

    /// <summary>
    /// Opens the game
    /// </summary>
    public void OpenGame()
    {
        // Sets not in level editor, clears level data, and loads the game
        LevelController.Instance.InLevelEditor = false;
        LevelController.Instance.ClearData();
        Application.LoadLevel(GAME_SCENE);
    }

    /// <summary>
    /// Opens the main menu
    /// </summary>
    public void OpenMainMenu()
    {
        Application.LoadLevel(MAIN_MENU_SCENE);
    }

    #endregion
}
