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
/// Class for serializing the data of a non-player object in a level
/// </summary>
[Serializable]
public class LevelObjectData
{
    #region Fields

    LevelObjectType type;   // The type of object this is

    GridPosition gridPosition;  // This object's position on the grid

    #endregion

    #region Properties

    /// <summary>
    /// Gets the object's position
    /// </summary>
    public Vector2 Position
    { get { return gridPosition.ToWorldPosition(); } }

    /// <summary>
    /// Gets the object's type
    /// </summary>
    public LevelObjectType Type
    { get { return type; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="gridPosition">The object's grid position</param>
    /// <param name="type">The object's type</param>
    public LevelObjectData(GridPosition gridPosition, LevelObjectType type)
    {
        this.gridPosition = gridPosition;
        this.type = type;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Makes a copy of the level object data
    /// </summary>
    /// <returns>the copy</returns>
    public LevelObjectData Clone()
    {
        return new LevelObjectData(gridPosition, type);
    }

    #endregion
}
