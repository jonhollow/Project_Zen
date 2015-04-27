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
    public GameObject blockPrefab;          // Prefab for the generic block object
    public GameObject playerStartPrefab;    // Prefab for the player start position object

    #endregion

    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // Initializes the game controller
        GameController.Instance.Initialize(playerPrefab, blockPrefab, playerStartPrefab);
    }

    #endregion
}
