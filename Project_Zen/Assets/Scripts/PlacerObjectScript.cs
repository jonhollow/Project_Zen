using UnityEngine;
using System.Collections;

/// <summary>
/// Script that controls the placer object
/// </summary>
public class PlacerObjectScript : MonoBehaviour
{
    #region Fields

    LevelObjectType currentType;    // The placer object's current object type
    SpriteRenderer spriteRenderer;  // The sprite renderer

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
	    // Sets the static reference to this object
        GameController.Instance.PlacerObject = gameObject;

        // Stores the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Deactivates the placer object
        gameObject.SetActive(false);
	}

	/// <summary>
    /// Update is called once per frame
	/// </summary>
	private void Update() 
    {
	    // Moves to the mouse position, clamped to the grid
        Vector2 position = GameController.Instance.MousePosition;
        position /= Constants.GRID_SIZE;
        position.x = (int)position.x;
        position.y = (int)position.y;
        position *= Constants.GRID_SIZE;
        transform.position = position;

        // Checks for cancel
        if (Input.GetKey(Constants.CANCEL_KEY))
        { gameObject.SetActive(false); }
	}

    /// <summary>
    /// OnMouseDown is called when the mouse clicks this object's collider
    /// </summary>
    private void OnMouseDown()
    {
        // Creates an object at the current location
        GameController.Instance.CreateLevelObject(currentType, transform.position, transform.rotation);

        // Deactivates the placer object
        gameObject.SetActive(false);
    }
    
    #endregion
}
