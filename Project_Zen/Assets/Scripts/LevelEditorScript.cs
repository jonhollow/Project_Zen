using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the overall level editor - may change significantly over time
/// </summary>
public class LevelEditorScript : LevelScript
{
    #region Fields

    static UndoHistory undoHistory; // The level editor's undo history

    #endregion

    #region Properties

    /// <summary>
    /// Gets and sets the placer object for the editor
    /// </summary>
    public static GameObject PlacerObject
    { get; set; }

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads the level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public override void Load(string filename)
    {
        // Adds a copy of the current state to the undo history
        undoHistory.StoreState(levelData.Clone());

        // Loads
        base.Load(filename);
    }

    /// <summary>
    /// Saves the level to the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public void Save(string filename)
    {
        Serializer.SerializeObject(filename, levelData);
    }

    /// <summary>
    /// Returns to the previous undo state
    /// </summary>
    public void Undo()
    {
        if (!undoHistory.Empty)
        {
            levelData = undoHistory.GetPreviousState();
            RestoreToLevelData();
        }
    }

    /// <summary>
    /// Creates an object in the level
    /// </summary>
    /// <param name="type">the type of object to create</param>
    /// <param name="position">the position at which to create the object</param>
    /// <param name="rotation">the rotation for the object</param>
    public static void CreateLevelObject(LevelObjectType type, Vector2 position, Quaternion rotation)
    {
        // Adds a copy of the current state to the undo history
        undoHistory.StoreState(levelData.Clone());

        // Creates an object at the provided position and adds it to the object list
        levelObjects.Add((GameObject)Instantiate(ObjectPrefabs[type], position, rotation));

        // Creates a unique ID for the new object
        string newID;
        do { newID = "Object" + Random.Range(int.MinValue, int.MaxValue); } 
        while (levelData.Objects.ContainsKey(newID));
        levelObjects[levelObjects.Count - 1].GetComponent<LevelObjectScript>().ID = newID;

        // Adds new object to the level data
        levelData.Objects.Add(newID, new LevelObjectData(position, type));
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // Sets in level editor
        InLevelEditor = true;

        // Initializes fields
        undoHistory = new UndoHistory();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Update is called once per frame
	/// </summary>
	private void Update() 
    {
        // Checks for undo on Z
        if (Input.GetKeyDown(KeyCode.Z))
        { Undo(); }
    }

    ///// <summary>
    ///// OnMouseDown is called when the mouse clicks this object's collider
    ///// NOTE: Probably not actually going to keep this method, just nice for testing
    ///// </summary>
    //private void OnMouseDown()
    //{
    //    CreateLevelObject(LevelObjectType.Block, LevelScript.GetMousePosition(), transform.rotation);
    //}

    #endregion
}
