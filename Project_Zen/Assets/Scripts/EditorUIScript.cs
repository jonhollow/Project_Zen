using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that holds level-editor-specifc UI behavior
/// </summary>
public class EditorUIScript : UIScript
{
    #region Fields

    public GameObject saveMenu; // The menu for choosing a save file

    public EditorToggleScript[] toggleOptions;  // Array of the editor toggles

    bool mouseOverUI = false;

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the save button being pressed
    /// </summary>
    public void SaveButtonPressed()
    {
        // Only saves if level has a filename, otherwise acts as save as
        if (LevelController.Instance.CurrentLevelName != "")
        { LevelController.Instance.SaveLevel(); }
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
    public void UndoButtonPressed()
    {
        LevelController.Instance.UndoLastChange();
    }

    /// <summary>
    /// Undoes the last undo
    /// </summary>
    public void RedoButtonPressed()
    {
        LevelController.Instance.RedoLastChange();
    }

    /// <summary>
    /// Handles the delete button being pressed
    /// </summary>
    public void DeleteButtonPressed()
    {
        GridDragObjectScript.DeleteSelection();
    }

    /// <summary>
    /// Handles the deselect button being pressed
    /// </summary>
    public void DeselectButtonPressed()
    {
        GridDragObjectScript.ClearSelection();
    }

    /// <summary>
    /// Handles the mouse entering the UI
    /// </summary>
    public void MouseEnterUI()
    {
        mouseOverUI = true;
    }

    /// <summary>
    /// Handles the mouse leaving the UI
    /// </summary>
    public void MouseExitUI()
    {
        mouseOverUI = false;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Checks for input while not paused
        if (!LevelController.Instance.Paused)
        {
            // Checks mouse not in UI for mouse controls
            if (!mouseOverUI)
            {
                // Checks for left mouse down for toggle action
                if (Input.GetMouseButtonDown(0))
                {
                    // Activates the selected toggle's effect
                    foreach (EditorToggleScript toggle in toggleOptions)
                    {
                        if (toggle.IsOn)
                        {
                            toggle.ActivateEffect();
                            break;
                        }
                    }
                }

                // Checks for middle mouse button for pan
                if (Input.GetMouseButtonDown(2))
                {
                    PanningObjectScript.StartPanning(2);
                }
            }

            // Checks for deselect key
            if (Input.GetKeyDown(KeyCode.Escape))
            { DeselectButtonPressed(); }

            // Checks for delete key
            if (Input.GetKeyDown(KeyCode.Delete))
            { DeleteButtonPressed(); }

            // Checks for ctrl-key shortcuts
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                // Checks for undo key
                if (Input.GetKeyDown(KeyCode.Z))
                { UndoButtonPressed(); }

                // Checks for redo key
                if (Input.GetKeyDown(KeyCode.Y))
                { RedoButtonPressed(); }

                // Checks for save key
                if (Input.GetKeyDown(KeyCode.S))
                { SaveButtonPressed(); }

                // Checks for cut key (just deletes for now)
                if (Input.GetKeyDown(KeyCode.X))
                { DeleteButtonPressed(); }
            }
        }
    }

    #endregion
}
