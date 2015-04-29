using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the placer object
/// </summary>
public class PlacerObjectScript : MonoBehaviour
{
    #region Fields

    LevelObjectType currentType;    // The placer object's current object type
    SpriteRenderer spriteRenderer;  // The sprite renderer
    GridPosition prevGridPosition;      
    GridPosition dragStartGridPosition;

    GameObject[,] previewBlocksGrid;    // The 2D array of preview blocks

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the placer object's current object type
    /// </summary>
    public LevelObjectType CurrentType
    { 
        get { return currentType; }
        set
        {
            currentType = value;

            // Changes sprite to match the new type
            spriteRenderer.sprite = GameController.Instance.ObjectPrefabs[currentType].GetComponent<SpriteRenderer>().sprite;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
	private void Start() 
    {
	    // Sets the reference to this object
        GameController.Instance.PlacerObject = gameObject;

        // Stores the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Initializes preview block array
        previewBlocksGrid = new GameObject[Constants.GRID_CELLS_Y, Constants.GRID_CELLS_X];

        // Deactivates the placer object
        gameObject.SetActive(false);
	}

    /// <summary>
    /// OnEnable is called when the object is enabled
    /// </summary>
    private void OnEnable()
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
	private void Update() 
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
            // Saves undo state
            GameController.Instance.AddUndoState();

            // Clears the preview block array, turns them into objects
            for (int i = 0; i < previewBlocksGrid.GetLength(0); i++)
            {
                for (int j = 0; j < previewBlocksGrid.GetLength(1); j++)
                {
                    // If there is a preview block here
                    if (previewBlocksGrid[i, j] != null)
                    {
                        // Creates an object at the preview block's grid location
                        GameController.Instance.CreateLevelObject(currentType, new GridPosition(i, j), transform.rotation);

                        // Removes the preview block
                        Destroy(previewBlocksGrid[i, j]);
                        previewBlocksGrid[i, j] = null; 
                    }
                }
            }

            // Deactivates placer object
            gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Gets the new grid position of the placer object
    /// </summary>
    private GridPosition GetNewGridPosition()
    {
        // Gets the new position (mouse position, clamped to grid)
        return GameController.WorldToGrid(GameController.Instance.MousePosition);
    }

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    private void UpdatePreviewBlocks()
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
                else if (previewBlocksGrid[i, j] == null && GameController.Instance.LevelGrid[i, j] == null &&
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
