using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the overall in-game level - may change significantly over time
/// </summary>
public class GameLevelScript : LevelScript
{
    #region Fields

    public GameObject playerPrefab; // Prefab for the player object

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Sets not in level editor
        InLevelEditor = false;

        // Stores the player prefab in the object dictionary
        objectPrefabs.Add(LevelObjectType.Player, playerPrefab);
    }

    #endregion
}
