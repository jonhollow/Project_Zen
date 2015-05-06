using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the placing object
/// </summary>
public class PlacingObjectScript : GridDragObjectScript
{
    #region Fields

    LevelObjectType currentType;    // The placer object's current object type
    SpriteRenderer spriteRenderer;  // The sprite renderer

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the placing object's current object type
    /// </summary>
    public LevelObjectType CurrentType
    { 
        get { return currentType; }
        set
        {
            currentType = value;

            // Changes sprite to match the new type
            placing.GetComponent<PlacingObjectScript>().spriteRenderer.sprite =
                GameController.Instance.ObjectPrefabs[currentType].GetComponent<SpriteRenderer>().sprite;
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
	protected override void Start() 
    {
        // Sets the reference to this object
        placing = gameObject;

        // Stores the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Start();
	}

    /// <summary>
    /// Ends the drag
    /// </summary>
    protected override void EndDrag()
    {
        // Saves the undo state
        GameController.Instance.AddUndoState();

        // Creates objects from the preview block array
        for (int i = 0; i < previewBlocksGrid.GetLength(0); i++)
        {
            for (int j = 0; j < previewBlocksGrid.GetLength(1); j++)
            {
                // If there is a preview block here
                if (previewBlocksGrid[i, j] != null)
                {
                    // Creates an object at the preview block's grid location
                    GameController.Instance.CreateLevelObject(currentType, new GridPosition(i, j), transform.rotation);
                }
            }
        }

        base.EndDrag();
    }

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    protected override void UpdatePreviewBlocks()
    {
        AddAndRemovePreviewBlocks(true);
    }

    #endregion
}
