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

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the load button being pressed
    /// </summary>
    public void LoadButtonPressed()
    {
        // Opens the load menu if there are any saved levels
        if (GameController.Instance.SavedLevels.Count > 0)
        { loadMenu.SetActive(true); }
    }

    /// <summary>
    /// Opens the level editor
    /// </summary>
    public void OpenLevelEditor()
    {
        GameController.Instance.OpenLevelEditor();
    }

    /// <summary>
    /// Opens the game
    /// </summary>
    public void OpenGame()
    {
        GameController.Instance.OpenGame();
    }

    /// <summary>
    /// Returns to the main menu
    /// </summary>
    public void ReturnToMainMenu()
    {
        Application.LoadLevel(Constants.MAIN_MENU_SCENE);
    }

    #endregion
}
