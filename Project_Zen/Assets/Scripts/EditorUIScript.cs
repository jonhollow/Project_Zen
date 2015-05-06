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

    public GameObject saveMenu;         // The menu for choosing a save file
    public Toggle[] placementOptions;   // Array of the placement toggles

    #endregion

    #region Public Methods

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

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    private void Update()
    {
        // Checks for mouse down while not paused
        if (!GameController.Instance.Paused && Input.GetMouseButtonDown(0))
        {
            // Checks if the mouse is in the grid
            GridPosition mouseGridPosition = GameController.WorldToGrid(GameController.Instance.MousePosition);
            if (mouseGridPosition.Row >= 0 &&
                mouseGridPosition.Row < Constants.GRID_ROWS &&
                mouseGridPosition.Column >= 0 &&
                mouseGridPosition.Column < Constants.GRID_COLUMNS)
            {
                // Checks which placement option is selected
                foreach (Toggle toggle in placementOptions)
                {
                    if (toggle.isOn)
                    {
                        GridDragObjectScript.ActivateDragObject(toggle.gameObject.GetComponent<PlaceButtonScript>().objectType);
                        break;
                    }
                }
            }
        }

        // Checks for deselect
        if (Input.GetKeyDown(KeyCode.Escape))
        { GridDragObjectScript.ClearSelection(); }

        // Checks for delete
        if (Input.GetKeyDown(KeyCode.Delete))
        { GridDragObjectScript.DeleteSelection(); }
    }

    #endregion
}
