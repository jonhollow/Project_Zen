using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Static class for the game's constants
/// </summary>
public static class Constants
{
    #region Fields

    public const float GRID_CELL_SIZE = 0.4f;   // The size of the grid cells for the level editor
    public const float GRID_X_OFFSET = -9.4f;   // The offset of the grid to the left of the center
    public const float GRID_Y_OFFSET = -4f;     // The offset of the grid down from the center
    public const int GRID_COLUMNS = 48;         // The number of columns in the grid
    public const int GRID_ROWS = 22;            // The number of rows in the grid

    public const string EDITOR_SCENE = "LevelEditor";   // The name of the level editor scene
    public const string GAME_SCENE = "Game";            // The name of the game scene
    public const string MAIN_MENU_SCENE = "MainMenu";   // The name of the main menu scene

    public const string LEVEL_NAMES_FILENAME = "lvlnms";    // The name of the file in which the list of level filenames is stored
    public const string LEVEL_FILES_FOLDER = "Levels/";     // The folder in which the level files are stored
    public const string FILE_SUFFIX = ".zd";                // The file type for save data

    #endregion
}
