using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that holds general UI behavior
/// </summary>
public class UIScript : MonoBehaviour
{
    #region Fields

    public GameObject saveMenu; // The menu for choosing a save file
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
    /// Handles the save button being pressed
    /// </summary>
    public void SaveButtonPressed()
    {
        // Only saves if level has a filename, otherwise acts as save as
        if (GameController.Instance.CurrentLevelName != "")
        { GameController.Instance.SaveLevel(); }
        else
        { SaveAsButtonPressed(); }
    }

    /// <summary>
    /// Handles the save as button being pressed
    /// </summary>
    public void SaveAsButtonPressed()
    {
        // Opens the save menu
        saveMenu.SetActive(true);
    }

    /// <summary>
    /// Returns to the previous undo state
    /// </summary>
    public void UndoLastChange()
    {
        GameController.Instance.UndoLastChange();
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
