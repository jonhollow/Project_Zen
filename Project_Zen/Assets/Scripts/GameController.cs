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

    LevelData levelData;        // The serializable level data
    UndoHistory undoHistory;    // The level editor's undo history

    Dictionary<LevelObjectType, GameObject> objectPrefabs;  // Dictionary of the level object prefabs
    Dictionary<GridPosition, GameObject> levelObjects;      // Dictionary of objects in the level
    SortedDictionary<string, LevelData> savedLevels;        // Dictionary of the saved levels

    //List<GameObject> levelObjects;      // The list of objects in the level
    List<string> savedLevelFilenames;   // List with the filenames of the saved levels

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
    /// Gets the sorted dictionary of saved levels
    /// </summary>
    public SortedDictionary<string, LevelData> SavedLevels
    { get { return savedLevels; } }

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
    /// Gets and sets the selection object for the editor
    /// </summary>
    public GameObject SelectionObject
    { get; set; }

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    public Vector2 MousePosition
    { get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); } }

    /// <summary>
    /// Gets or sets whether the game is paused
    /// </summary>
    public bool Paused
    { get; set; }

    /// <summary>
    /// Gets or sets the name of the currently loaded level
    /// </summary>
    public string CurrentLevelName
    { get; set; }

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
            Paused = false;
            objectPrefabs = new Dictionary<LevelObjectType, GameObject>();
            levelData = new LevelData();
            levelObjects = new Dictionary<GridPosition, GameObject>();
            undoHistory = new UndoHistory();
            savedLevels = new SortedDictionary<string, LevelData>();
            CurrentLevelName = "";

            // Adds objects to the object prefab dictionary
            objectPrefabs.Add(LevelObjectType.Player, playerPrefab);
            objectPrefabs.Add(LevelObjectType.PreviewBlock, previewBlockPrefab);
            objectPrefabs.Add(LevelObjectType.Block, blockPrefab);
            objectPrefabs.Add(LevelObjectType.PlayerStart, playerStartPrefab);

            // Loads the saved levels, if any
            savedLevelFilenames = (List<string>)Serializer.Deserialize(Constants.LEVEL_NAMES_FILENAME);
            if (savedLevelFilenames != null)
            {
                // Loads the levels into the saved levels dictionary
                for (int i = savedLevelFilenames.Count - 1; i >= 0; i--)
                {
                    // Checks to make sure the level wasn't deleted offline
                    LevelData loadedLevel = LoadLevelFromFile(savedLevelFilenames[i]);
                    if (loadedLevel != null)
                    { savedLevels.Add(savedLevelFilenames[i], loadedLevel); }
                    else
                    {
                        savedLevelFilenames.RemoveAt(i);
                        Serializer.Serialize(Constants.LEVEL_NAMES_FILENAME, savedLevelFilenames);
                    }
                }
            }
            else
            {
                savedLevelFilenames = new List<string>();
            }
        }
    }

    /// <summary>
    /// Loads the level from the current file name
    /// </summary>
    public void LoadLevel()
    {
        // Adds a copy of the current state to the undo history
        AddUndoState();

        // Reloads the level data if possible
        LevelData loadedData = (LevelData)Serializer.Deserialize(Constants.LEVEL_FILES_FOLDER + CurrentLevelName);
        if (loadedData != null)
        {
            levelData = loadedData;
            RestoreToLevelData();
        }
    }

    /// <summary>
    /// Saves the level to the current level file name
    /// </summary>
    public void SaveLevel()
    {
        // Saves the level data
        Serializer.Serialize(Constants.LEVEL_FILES_FOLDER + CurrentLevelName, levelData);

        // If the file is new, add it to the saved levels dictionary and level filenames list
        if (!savedLevelFilenames.Contains(CurrentLevelName))
        {
            savedLevels.Add(CurrentLevelName, levelData);
            savedLevelFilenames.Add(CurrentLevelName);
            Serializer.Serialize(Constants.LEVEL_NAMES_FILENAME, savedLevelFilenames);
        }
    }

    /// <summary>
    /// Deletes the current level file
    /// </summary>
    public void DeleteLevel()
    {
        // Deletes the file
        Serializer.DeleteFile(Constants.LEVEL_FILES_FOLDER + CurrentLevelName);

        // Removes the file from the saved levels dictionary and level filenames list
        savedLevels.Remove(CurrentLevelName);
        savedLevelFilenames.Remove(CurrentLevelName);
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

            // Clears the selection
            GridDragObjectScript.ClearSelection();
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
        levelObjects.Add(gridPosition, (GameObject)MonoBehaviour.Instantiate(objectPrefabs[type], levelData.Grid[gridPosition.Row, 
            gridPosition.Column].Position, rotation));
    }

    /// <summary>
    /// Destroys the level object at the given position
    /// </summary>
    /// <param name="gridPosition">the position</param>
    public void DestroyLevelObject(GridPosition gridPosition)
    {
        MonoBehaviour.Destroy(levelObjects[gridPosition]);
        levelObjects.Remove(gridPosition);
        levelData.Grid[gridPosition.Row, gridPosition.Column] = null;
    }

    /// <summary>
    /// Adds the current state to the undo history
    /// </summary>
    public void AddUndoState()
    {
        undoHistory.StoreState(levelData.Clone());
    }

    /// <summary>
    /// Adds the given object to the level
    /// </summary>
    /// <param name="gridPosition">the object's grid position</param>
    /// <param name="objectToAdd">the object</param>
    public void AddObjectToLevel(GridPosition gridPosition, GameObject objectToAdd)
    {
        levelObjects.Add(gridPosition, objectToAdd);
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
        worldLocation /= Constants.GRID_CELL_SIZE;

        return new GridPosition(Mathf.RoundToInt(worldLocation.y), Mathf.RoundToInt(worldLocation.x));
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Restores the level to the level data
    /// </summary>
    private void RestoreToLevelData()
    {
        // Destroys everything currently in the scene
        foreach (KeyValuePair<GridPosition, GameObject> ob in levelObjects)
        { MonoBehaviour.Destroy(ob.Value); }
        levelObjects.Clear();

        // Reconstructs the scene
        for (int i = 0; i < levelData.Grid.GetLength(0); i++)
        {
            for (int j = 0; j < levelData.Grid.GetLength(1); j++)
            {
                // If there is an object here
                if (levelData.Grid[i, j] != null)
                {
                    // Creates the object
                    levelObjects.Add(new GridPosition(i, j), (GameObject)MonoBehaviour.Instantiate(objectPrefabs[levelData.Grid[i, j].Type], 
                        levelData.Grid[i, j].Position, Quaternion.Euler(Vector3.zero)));
                }
            }
        }
    }

    /// <summary>
    /// Loads a level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    /// <returns>the loaded level, or null if level was not found</returns>
    private LevelData LoadLevelFromFile(string filename)
    {
        return (LevelData)Serializer.Deserialize(Constants.LEVEL_FILES_FOLDER + filename);
    }

    #endregion
}
