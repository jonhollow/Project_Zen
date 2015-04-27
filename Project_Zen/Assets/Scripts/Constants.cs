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

    public const string EDITOR_SCENE = "LevelEditor";   // The name of the level editor scene
    public const string GAME_SCENE = "Game";            // The name of the game scene
    public const string MAIN_MENU_SCENE = "MainMenu";   // The name of the main menu scene

    #endregion
}
