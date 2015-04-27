using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that holds general UI behavior
/// </summary>
public class UIScript : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    /// Loads the level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public void LoadLevel(string filename)
    {
        GameController.Instance.LoadLevel(filename);
    }

    /// <summary>
    /// Saves the level to the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public void SaveLevel(string filename)
    {
        GameController.Instance.SaveLevel(filename);
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
