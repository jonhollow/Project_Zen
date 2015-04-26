using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Static class for the game's constants
/// </summary>
public static class Constants
{
    #region Fields

    public const float GRID_SIZE = 0.5f;   // The size of the grid cells for the level editor

    public const KeyCode CANCEL_KEY = KeyCode.Escape;   // Key to cancel placement of an object

    // Input constants
    public const string MOVE_INPUT_AXIS = "Horizontal";
    public const string JUMP_INPUT_AXIS = "Jump";

    #endregion
}
