using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Abstract parent script that provides shared behavior between the grid drag objects
/// </summary>
public abstract class GridDragObjectScript : MonoBehaviour
{
    #region Fields

    protected GridPosition prevGridPosition;        // The object's previous position on the grid
    protected GridPosition dragStartGridPosition;   // The position the drag started at

    protected static GameObject[,] previewBlocksGrid;   // The 2D array of preview blocks
    protected static GameObject placing;
    protected static GameObject selecting;
    protected static GameObject moving;

    #endregion

    #region Public Methods

    /// <summary>
    /// Activates the appropriate drag object
    /// </summary>
    /// <param name="type">the type of object for placement</param>
    public static void ActivateDragObject(LevelObjectType type)
    {
        // Only starts drag if stuff has been initialized
        if (previewBlocksGrid != null)
        {
            // Checks which object to activate
            GridPosition mousePosition = GetNewGridPosition();
            if (previewBlocksGrid[mousePosition.Row, mousePosition.Column] != null)
            { moving.SetActive(true); }
            else if (GameController.Instance.LevelGrid[mousePosition.Row, mousePosition.Column] != null)
            { selecting.SetActive(true); }
            else
            {
                placing.SetActive(true);
                placing.GetComponent<PlacingObjectScript>().CurrentType = type;
            }
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start() 
    {
        // Initializes preview block array
        previewBlocksGrid = new GameObject[Constants.GRID_ROWS, Constants.GRID_COLUMNS];

        // Deactivates the object
        gameObject.SetActive(false);
	}

    /// <summary>
    /// OnEnable is called when the object is enabled
    /// </summary>
    protected void OnEnable()
    {
        // Only starts drag if it's been initialized
        if (previewBlocksGrid != null)
        {
            // Stores initial location
            prevGridPosition = GetNewGridPosition();
            dragStartGridPosition = prevGridPosition;
            transform.position = prevGridPosition.ToWorldPosition();

            // Sets up preview blocks
            UpdatePreviewBlocks();
        }
    }

	/// <summary>
    /// Update is called once per frame
	/// </summary>
    protected void Update() 
    {
        // Checks if the position has changed
        GridPosition newGridPosition = GetNewGridPosition();
        if (newGridPosition != prevGridPosition)
        {
            // Moves to the new position
            prevGridPosition = newGridPosition;
            transform.position = prevGridPosition.ToWorldPosition();
            
            // Updates preview blocks
            UpdatePreviewBlocks();
        }

        // Checks for drag ended
        if (!Input.GetMouseButton(0))
        {
            EndDrag();

            // Deactivates placer object
            gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Gets the new grid position from the mouse position
    /// </summary>
    protected static GridPosition GetNewGridPosition()
    {
        // Gets the new position (mouse position, clamped to grid)
        return GameController.WorldToGrid(GameController.Instance.MousePosition);
    }
    /// <summary>
    /// Ends the drag
    /// </summary>
    protected virtual void EndDrag()
    {
        // Saves undo state
        GameController.Instance.AddUndoState();
    }

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    protected abstract void UpdatePreviewBlocks();

    /// <summary>
    /// Adds or removes the appropriate amount of preview blocks
    /// For use by the placing and selecting objects
    /// </summary>
    /// <param name="place">whether or not this is the placing object</param>
    protected virtual void AddAndRemovePreviewBlocks(bool place)
    {
        // Updates the grid
        for (int i = 0; i < previewBlocksGrid.GetLength(0); i++)
        {
            for (int j = 0; j < previewBlocksGrid.GetLength(1); j++)
            {
                // Checks if a block should be removed
                if (previewBlocksGrid[i, j] != null &&
                    (i < Mathf.Min(prevGridPosition.Row, dragStartGridPosition.Row) ||
                    i > Mathf.Max(prevGridPosition.Row, dragStartGridPosition.Row) ||
                    j < Mathf.Min(prevGridPosition.Column, dragStartGridPosition.Column) ||
                    j > Mathf.Max(prevGridPosition.Column, dragStartGridPosition.Column)))
                {
                    // Removes the preview block
                    Destroy(previewBlocksGrid[i, j]);
                    previewBlocksGrid[i, j] = null;
                }
                // Checks if a block should be added
                else if (previewBlocksGrid[i, j] == null && (GameController.Instance.LevelGrid[i, j] == null) == place &&
                    i >= Mathf.Min(prevGridPosition.Row, dragStartGridPosition.Row) &&
                    i <= Mathf.Max(prevGridPosition.Row, dragStartGridPosition.Row) &&
                    j >= Mathf.Min(prevGridPosition.Column, dragStartGridPosition.Column) &&
                    j <= Mathf.Max(prevGridPosition.Column, dragStartGridPosition.Column))
                {
                    // Adds a preview block
                    previewBlocksGrid[i, j] = (GameObject)Instantiate(GameController.Instance.ObjectPrefabs[LevelObjectType.PreviewBlock],
                        new GridPosition(i, j).ToWorldPosition(), transform.rotation);
                }
            }
        }
    }

    #endregion
}
