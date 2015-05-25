using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script that controls the save menu
/// </summary>
public class SaveMenuScript : PauseMenuWFilesScript
{
    #region Fields

    public InputField inputField;   // The menu's input field

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the save button being pressed
    /// </summary>
    public void SaveButtonPressed()
    {
        // Checks if a level name has been chosen
        if (LevelController.Instance.CurrentLevelName != "")
        {
            // Hides the save menu
            gameObject.SetActive(false);

            // Saves the level
            LevelController.Instance.SaveLevel();
        }
    }

    /// <summary>
    /// Handles the input field value being changed
    /// </summary>
    /// <param name="input">the input</param>
    public void HandleInputValueChanged(string input)
    {
        LevelController.Instance.CurrentLevelName = input;
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Handles clicking on a level file option
    /// </summary>
    /// <param name="value">the new value</param>
    protected override void LevelFileValueChanged(bool value)
    {
        base.LevelFileValueChanged(value);

        // Updates the input field text
        inputField.text = LevelController.Instance.CurrentLevelName;
    }

    #endregion
}
