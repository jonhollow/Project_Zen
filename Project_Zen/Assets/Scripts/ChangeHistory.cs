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
/// Class for holding the undo and redo histories for the level editor
/// </summary>
public class ChangeHistory
{
    #region Fields

    Stack<LevelData> undoStates;    // Stack of previous level states
    Stack<LevelData> redoStates;    // Stack of recently-undone level states

    #endregion

    #region Properties

    /// <summary>
    /// Gets whether or not there are any undo states
    /// </summary>
    public bool HasUndo
    { get { return undoStates.Count > 0; } }

    /// <summary>
    /// Gets whether or not there are any redo states
    /// </summary>
    public bool HasRedo
    { get { return redoStates.Count > 0; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public ChangeHistory()
    {
        undoStates = new Stack<LevelData>();
        redoStates = new Stack<LevelData>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Stores the given state as the last undo state
    /// </summary>
    /// <param name="currentState">the current state</param>
    public void StoreState(LevelData currentState)
    {
        // Adds the state to the stack
        undoStates.Push(currentState);

        // Clears the redo history
        redoStates.Clear();
    }

    /// <summary>
    /// Gets the previous undo state
    /// </summary>
    /// <param name="currentState">the current state, to store in the redo history</param>
    /// <returns>the previous undo state</returns>
    public LevelData GetPreviousState(LevelData currentState)
    {
        // Stores the current state in the redo history
        redoStates.Push(currentState);

        // Pulls the first item off the undo stack and returns it
        return undoStates.Pop();
    }

    /// <summary>
    /// Gets the last redo state
    /// </summary>
    /// <param name="currentState">the current state, to store in the undo history</param>
    /// <returns>the last redo state</returns>
    public LevelData GetLastRedoState(LevelData currentState)
    {
        // Stores the current state in the undo history
        undoStates.Push(currentState);

        // Pulls the first item off the stack and returns it
        return redoStates.Pop();
    }

    /// <summary>
    /// Clears the undo history
    /// </summary>
    public void Clear()
    {
        undoStates.Clear();
        redoStates.Clear();
    }

    #endregion
}
