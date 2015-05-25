using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script that controls the load menu
/// </summary>
public class LoadMenuScript : PauseMenuWFilesScript
{
    #region Public Methods

    /// <summary>
    /// Handles the load button being pressed
    /// </summary>
    public void LoadButtonPressed()
    {
        // Checks if a level has been chosen
        if (LevelController.Instance.CurrentLevelName != "")
        {
            // Hides the load menu
            gameObject.SetActive(false);

            // Loads the level
            LevelController.Instance.LoadLevel();
        }
    }

    /// <summary>
    /// Handles the delete file button being pressed
    /// </summary>
    public void DeleteFileButtonPressed()
    {
        // Checks if a level has been chosen
        if (LevelController.Instance.CurrentLevelName != "")
        {
            // Hides the load menu
            gameObject.SetActive(false);

            // Deletes the level
            LevelController.Instance.DeleteLevel();
            LevelController.Instance.CurrentLevelName = "";
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Updates the menu on enable
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();

        // Resets the level name
        LevelController.Instance.CurrentLevelName = "";
    }

    #endregion
}
