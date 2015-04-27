using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Class for holding the undo history for the level editor
/// </summary>
public class UndoHistory
{
    #region Fields

    Stack<LevelData> prevStates;    // Stack of previous level states

    #endregion

    #region Properties

    /// <summary>
    /// Gets whether or not the undo history is empty
    /// </summary>
    public bool Empty
    { get { return prevStates.Count == 0; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public UndoHistory()
    {
        prevStates = new Stack<LevelData>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Stores the given state as the last undo state
    /// </summary>
    /// <param name="levelData"></param>
    public void StoreState(LevelData levelData)
    {
        // Adds the state to the stack
        prevStates.Push(levelData);
    }

    /// <summary>
    /// Gets the previous undo state
    /// </summary>
    /// <returns>the previous undo state</returns>
    public LevelData GetPreviousState()
    {
        // Pulls the first item off the stack and returns it
        return prevStates.Pop();
    }

    /// <summary>
    /// Clears the undo history
    /// </summary>
    public void Clear()
    {
        prevStates.Clear();
    }

    #endregion
}
