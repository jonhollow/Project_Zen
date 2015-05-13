using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the moving object
/// </summary>
public class MovingObjectScript : GridDragObjectScript
{
    #region Fields

    LevelObjectType[,] startPositions;  // Array that stores where the blocks started before the movement

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
	protected override void Start() 
    {
        // Sets the reference to this object
        moving = gameObject;

        // Creates start positions array
        startPositions = new LevelObjectType[previewBlocksGrid.GetLength(0), previewBlocksGrid.GetLength(1)];

        base.Start();
	}

    /// <summary>
    /// Starts the drag
    /// </summary>
    protected override void StartDrag()
    {
        // Saves the undo state
        GameController.Instance.AddUndoState();

        // Fills the start positions array
        for (int i = 0; i < startPositions.GetLength(0); i++)
        {
            for (int j = 0; j < startPositions.GetLength(1); j++)
            { 
                if (previewBlocksGrid[i, j] != null)
                { startPositions[i, j] = GameController.Instance.LevelGrid[i, j].Type; }
                else if (GameController.Instance.LevelGrid[i, j] != null)
                { startPositions[i, j] = LevelObjectType.Unknown; }
                else 
                { startPositions[i, j] = LevelObjectType.Empty; }
            }
        }

        base.StartDrag();
    }

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    protected override void UpdatePreviewBlocks()
    {
        // Gets the difference between the start and current positions
        int differenceRows = prevGridPosition.Row - dragStartGridPosition.Row;
        int differenceCols = prevGridPosition.Column - dragStartGridPosition.Column;

        // Updates the grid
        for (int i = 0; i < previewBlocksGrid.GetLength(0); i++)
        {
            int shiftedRow = i - differenceRows;
            for (int j = 0; j < previewBlocksGrid.GetLength(1); j++)
            {
                // Checks if a block should be removed
                int shiftedCol = j - differenceCols;
                if (previewBlocksGrid[i, j] != null && (shiftedRow < 0 || shiftedRow >= previewBlocksGrid.GetLength(0) ||
                    shiftedCol < 0 || shiftedCol >= previewBlocksGrid.GetLength(1) ||
                    startPositions[shiftedRow, shiftedCol] == LevelObjectType.Empty ||
                    startPositions[shiftedRow, shiftedCol] == LevelObjectType.Unknown))
                {
                    // Removes the preview block
                    Destroy(previewBlocksGrid[i, j]);
                    previewBlocksGrid[i, j] = null;

                    // Checks if the object should be removed
                    if (startPositions[i, j] != LevelObjectType.Unknown)
                    { GameController.Instance.DestroyLevelObject(new GridPosition(i, j)); }
                }
                // Checks if a block should be added
                else if (previewBlocksGrid[i, j] == null && shiftedRow >= 0 && shiftedRow < previewBlocksGrid.GetLength(0) &&
                    shiftedCol >= 0 && shiftedCol < previewBlocksGrid.GetLength(1) &&
                    startPositions[shiftedRow, shiftedCol] != LevelObjectType.Empty &&
                    startPositions[shiftedRow, shiftedCol] != LevelObjectType.Unknown)
                {
                    // Adds a preview block
                    previewBlocksGrid[i, j] = (GameObject)Instantiate(GameController.Instance.ObjectPrefabs[LevelObjectType.PreviewBlock],
                        new GridPosition(i, j).ToWorldPosition(), transform.rotation);

                    // Checks if an object should be added
                    if (GameController.Instance.LevelGrid[i, j] == null)
                    {
                        GameController.Instance.CreateLevelObject(startPositions[shiftedRow, shiftedCol],
                            new GridPosition(i, j), transform.rotation);
                    }
                }
            }
        }
    }

    #endregion
}
