using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Static class for the game's constants
/// </summary>
public static class Constants
{
    #region Fields

    public const float GRID_SIZE = 0.5f;    // The size of the grid cells for the level editor
    public const float GRID_X_OFFSET = -9.25f;    
    public const float GRID_Y_OFFSET = -3.6f;
    public const int GRID_CELLS_X = (int)(19 / GRID_SIZE); // The number of grid cells horizontally
    public const int GRID_CELLS_Y = (int)(8 / GRID_SIZE);  // The number of grid cells vertically

    public const KeyCode CANCEL_KEY = KeyCode.Escape;   // Key to cancel placement of an object

    public const string EDITOR_SCENE = "LevelEditor";   // The name of the level editor scene
    public const string GAME_SCENE = "Game";            // The name of the game scene
    public const string MAIN_MENU_SCENE = "MainMenu";   // The name of the main menu scene

    #endregion
}
