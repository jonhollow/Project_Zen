using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Specialized UI script that initializes the game controller
/// </summary>
public class MainMenuScript : UIScript
{
    #region Fields

    public GameObject playerPrefab;         // Prefab for the player object
    public GameObject previewBlockPrefab;   // Prefab for the preview block
    public GameObject blockPrefab;          // Prefab for the generic block object
    public GameObject playerStartPrefab;    // Prefab for the player start position object

    public Sprite[] blockTileSideSprites;
    public Sprite[] blockTileCornerSprites; 

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // Initializes the game controller
        GameController.Instance.Initialize(playerPrefab, previewBlockPrefab, blockPrefab, 
            playerStartPrefab, blockTileSideSprites, blockTileCornerSprites);
    }

    #endregion
}
