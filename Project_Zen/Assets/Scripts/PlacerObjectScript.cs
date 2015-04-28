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
    Vector2 prevPosition;           // The previous position
    Vector2 dragStartPosition;      // The start position of the drag

    List<List<GameObject>> previewBlocks;   // The 2D list of preview blocks

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

        // Initializes preview block list
        previewBlocks = new List<List<GameObject>>();

        // Deactivates the placer object
        gameObject.SetActive(false);
	}

    /// <summary>
    /// OnEnable is called when the object is enabled
    /// </summary>
    private void OnEnable()
    {
        // Only starts drag if it's been initialized
        if (previewBlocks != null)
        {
            // Stores initial location
            transform.position = GetNewPosition();
            dragStartPosition = transform.position;

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
        Vector2 position = GetNewPosition();
        if (position != prevPosition)
        {
            // Moves to the new position
            transform.position = position;
            prevPosition = position;

            // Updates preview blocks
            UpdatePreviewBlocks();
        }

        // Checks for drag ended
        if (!Input.GetMouseButton(0))
        {
            // Saves undo state
            GameController.Instance.AddUndoState();

            // Clears the preview block list, turns them into objects
            for (int i = previewBlocks.Count - 1; i >= 0; i--)
            {
                for (int j = previewBlocks[i].Count - 1; j >= 0; j--)
                {
                    // Creates an object at the preview block's location
                    GameController.Instance.CreateLevelObject(currentType, previewBlocks[i][j].transform.position, transform.rotation);

                    // Removes the preview block
                    MonoBehaviour.Destroy(previewBlocks[i][j]);
                    previewBlocks[i].RemoveAt(j);
                }
            }

            // Deactivates placer object
            gameObject.SetActive(false);
        }
	}

    /// <summary>
    /// Gets the new position of the placer object
    /// </summary>
    private Vector2 GetNewPosition()
    {
        // Gets the new position (mouse position, clamped to grid)
        Vector2 position = GameController.Instance.MousePosition;
        position /= Constants.GRID_SIZE;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position *= Constants.GRID_SIZE;

        return position;
    }

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    private void UpdatePreviewBlocks()
    {
        // Gets needed rows and columns
        int xDirection = (int)Mathf.Sign(transform.position.x - dragStartPosition.x);
        int yDirection = (int)Mathf.Sign(transform.position.y - dragStartPosition.y);
        int neededRows = (int)(Mathf.Abs(transform.position.y - dragStartPosition.y) / Constants.GRID_SIZE) + 1;
        int neededColumns = (int)(Mathf.Abs(transform.position.x - dragStartPosition.x) / Constants.GRID_SIZE) + 1;

        // Checks for need more rows
        while (previewBlocks.Count < neededRows)
        {
            previewBlocks.Add(new List<GameObject>());

            // Populates the new row
            while (previewBlocks[previewBlocks.Count - 1].Count < neededColumns)
            {
                previewBlocks[previewBlocks.Count - 1].Add((GameObject)Instantiate(GameController.Instance.ObjectPrefabs[LevelObjectType.PreviewBlock],
                    new Vector2(dragStartPosition.x + (previewBlocks[previewBlocks.Count - 1].Count * Constants.GRID_SIZE * xDirection),
                        dragStartPosition.y + ((previewBlocks.Count - 1) * Constants.GRID_SIZE * yDirection)), transform.rotation));
            }

        }

        // Checks for need fewer rows
        while (previewBlocks.Count > neededRows)
        {
            // Clears the last row
            while (previewBlocks[previewBlocks.Count - 1].Count > 0)
            {
                MonoBehaviour.Destroy(previewBlocks[previewBlocks.Count - 1][0]);
                previewBlocks[previewBlocks.Count - 1].RemoveAt(0);
            }

            previewBlocks.RemoveAt(previewBlocks.Count - 1);
        }

        // Checks for need more columns
        if (previewBlocks[0].Count < neededColumns)
        {
            // Loops through each row adding more columns
            for (int i = 0; i < previewBlocks.Count; i++)
            {
                while (previewBlocks[i].Count < neededColumns)
                {
                    previewBlocks[i].Add((GameObject)Instantiate(GameController.Instance.ObjectPrefabs[LevelObjectType.PreviewBlock],
                        new Vector2(dragStartPosition.x + (previewBlocks[i].Count * Constants.GRID_SIZE * xDirection),
                            dragStartPosition.y + (i * Constants.GRID_SIZE * yDirection)), transform.rotation));
                }
            }
        }

        // Checks for need fewer columns
        else if (previewBlocks[0].Count > neededColumns)
        {
            // Loops through each row removing columns
            for (int i = 0; i < previewBlocks.Count; i++)
            {
                while (previewBlocks[i].Count > neededColumns)
                {
                    MonoBehaviour.Destroy(previewBlocks[i][previewBlocks[i].Count - 1]);
                    previewBlocks[i].RemoveAt(previewBlocks[i].Count - 1);
                }
            }
        }
    }
    
    #endregion
}
