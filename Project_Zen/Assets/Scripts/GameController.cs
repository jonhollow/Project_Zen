using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Singleton class that controls various aspects of levels and the level editor
/// </summary>
public class GameController
{
    #region Fields

    static GameController instance; // Singleton instance of the controller

    LevelData levelData;   // The serializable level data

    UndoHistory undoHistory;    // The level editor's undo history

    List<GameObject> levelObjects; // The list of objects in the level

    Dictionary<LevelObjectType, GameObject> objectPrefabs;   // Dictionary of the level object prefabs

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton game controller instance
    /// </summary>
    public static GameController Instance
    {
        get
        {
            if (instance == null)
            { instance = new GameController(); }
            return instance;
        }
    }

    /// <summary>
    /// Gets the current level object grid
    /// </summary>
    public LevelObjectData[,] LevelGrid
    { get { return levelData.Grid; } }

    /// <summary>
    /// Gets the dictionary of level object prefabs
    /// </summary>
    public Dictionary<LevelObjectType, GameObject> ObjectPrefabs
    { get { return objectPrefabs; } }

    /// <summary>
    /// Gets or sets whether the game is in the level editor or an actual level
    /// </summary>
    public bool InLevelEditor
    { get; set; }

    /// <summary>
    /// Gets and sets the placer object for the editor
    /// </summary>
    public GameObject PlacerObject
    { get; set; }

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    #endregion

    #region Constructors

    /// <summary>
    /// Private constructor
    /// </summary>
    private GameController()
    {

    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initializes the game controller
    /// </summary>
    /// <param name="blockPrefab">the prefab for the block object</param>
    /// <param name="playerStartPrefab">the prefab for the player start position object</param>
    public void Initialize(GameObject playerPrefab, GameObject previewBlockPrefab, GameObject blockPrefab, GameObject playerStartPrefab)
    {
        // Only initializes if needed
        if (objectPrefabs == null)
        {
            // Initializes fields
            objectPrefabs = new Dictionary<LevelObjectType, GameObject>();
            levelData = new LevelData();
            levelObjects = new List<GameObject>();
            undoHistory = new UndoHistory();

            // Adds objects to the object prefab dictionary
            objectPrefabs.Add(LevelObjectType.Player, playerPrefab);
            objectPrefabs.Add(LevelObjectType.PreviewBlock, previewBlockPrefab);
            objectPrefabs.Add(LevelObjectType.Block, blockPrefab);
            objectPrefabs.Add(LevelObjectType.PlayerStart, playerStartPrefab);
        }
    }

    /// <summary>
    /// Loads the level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public void LoadLevel(string filename)
    {
        // Adds a copy of the current state to the undo history
        AddUndoState();

        // Reloads the level data if possible
        LevelData loadedData = (LevelData)Serializer.DeserializeObject(filename);
        if (loadedData != null)
        {
            levelData = loadedData;
            RestoreToLevelData();
        }
    }

    /// <summary>
    /// Saves the level to the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public void SaveLevel(string filename)
    {
        Serializer.SerializeObject(filename, levelData);
    }

    /// <summary>
    /// Returns to the previous undo state
    /// </summary>
    public void UndoLastChange()
    {
        if (!undoHistory.Empty)
        {
            levelData = undoHistory.GetPreviousState();
            RestoreToLevelData();
        }
    }

    /// <summary>
    /// Opens the level editor
    /// </summary>
    public void OpenLevelEditor()
    {
        // Sets in level editor, clears undo history & level data, and loads the level editor
        InLevelEditor = true;
        undoHistory.Clear();
        levelData.Clear();
        Application.LoadLevel(Constants.EDITOR_SCENE);
    }

    /// <summary>
    /// Opens the game
    /// </summary>
    public void OpenGame()
    {
        // Sets not in level editor, clears level data, and loads the game
        InLevelEditor = false;
        levelData.Clear();
        Application.LoadLevel(Constants.GAME_SCENE);
    }

    /// <summary>
    /// Creates an object in the level
    /// </summary>
    /// <param name="type">the type of object to create</param>
    /// <param name="gridPosition">the grid position</param>
    /// <param name="rotation">the rotation for the object</param>
    public void CreateLevelObject(LevelObjectType type, GridPosition gridPosition, Quaternion rotation)
    {
        // Adds the object into the level data at its grid position
        levelData.Grid[gridPosition.Row, gridPosition.Column] = new LevelObjectData(gridPosition, type);

        // Creates an object at its world position and adds it to the object list
        levelObjects.Add((GameObject)MonoBehaviour.Instantiate(objectPrefabs[type], levelData.Grid[gridPosition.Row, 
            gridPosition.Column].Position, rotation));
    }

    /// <summary>
    /// Adds the current state to the undo history
    /// </summary>
    public void AddUndoState()
    {
        undoHistory.StoreState(levelData.Clone());
    }

    /// <summary>
    /// Converts world coordinates to grid coordinates
    /// </summary>
    /// <param name="worldLocation">the world location</param>
    /// <returns>the grid location</returns>
    public static GridPosition WorldToGrid(Vector2 worldLocation)
    {
        worldLocation.x -= Constants.GRID_X_OFFSET;
        worldLocation.y -= Constants.GRID_Y_OFFSET;
        worldLocation /= Constants.GRID_SIZE;
        worldLocation.x = Mathf.Clamp(Mathf.Round(worldLocation.x), 0, Constants.GRID_CELLS_X);
        worldLocation.y = Mathf.Clamp(Mathf.Round(worldLocation.y), 0, Constants.GRID_CELLS_Y);

        return new GridPosition((int)worldLocation.y, (int)worldLocation.x);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Restores the level to the level data
    /// </summary>
    private void RestoreToLevelData()
    {
        // Destroys everything currently in the scene
        while (levelObjects.Count > 0)
        {
            MonoBehaviour.Destroy(levelObjects[0]);
            levelObjects.RemoveAt(0);
        }

        // Reconstructs the scene
        for (int i = 0; i < levelData.Grid.GetLength(0); i++)
        {
            for (int j = 0; j < levelData.Grid.GetLength(1); j++)
            {
                // If there is an object here
                if (levelData.Grid[i, j] != null)
                {
                    // Creates the object
                    levelObjects.Add((GameObject)MonoBehaviour.Instantiate(objectPrefabs[levelData.Grid[i, j].Type], 
                        levelData.Grid[i, j].Position, Quaternion.Euler(Vector3.zero)));
                }
            }
        }
    }

    #endregion
}
