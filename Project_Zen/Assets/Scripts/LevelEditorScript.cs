using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the overall level editor - may change significantly over time
/// </summary>
public class LevelEditorScript : LevelScript
{
    #region Fields

    UndoHistory undoHistory;    // The level editor's undo history

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

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected override void Start()
    {
        base.Start();

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

    /// <summary>
    /// OnMouseDown is called when the mouse clicks this object's collider
    /// NOTE: Probably not actually going to keep this method, just nice for testing
    /// </summary>
    private void OnMouseDown()
    {
        // Adds a copy of the current state to the undo history
        undoHistory.StoreState(levelData.Clone());

        // Gets the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Creates a block where the mouse is and adds it to the object list
        levelObjects.Add((GameObject)Instantiate(block, mousePosition, transform.rotation));

        // Creates a unique ID for the new object
        string newID = "Object" + Random.Range(int.MinValue, int.MaxValue);
        while (levelData.Objects.ContainsKey(newID))
        { newID = "Object" + Random.Range(int.MinValue, int.MaxValue); }
        levelObjects[levelObjects.Count - 1].GetComponent<LevelObjectScript>().ID = newID;

        // Adds the block to the level data
        levelData.Objects.Add(newID, new LevelObjectData(mousePosition, LevelObjectType.Block));
    }

    #endregion
}
