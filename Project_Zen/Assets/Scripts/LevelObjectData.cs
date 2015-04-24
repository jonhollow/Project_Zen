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
/// A class for serializing the data of a non-player object in a level
/// </summary>
[Serializable]
public class LevelObjectData
{
    #region Fields

    LevelObjectType type;   // The type of object this is

    float x;    // The x position of this object in the level
    float y;    // The y position of this object in the level

    #endregion

    #region Properties

    /// <summary>
    /// Gets the object's position
    /// </summary>
    public Vector2 Position
    { get { return new Vector2(x, y); } }

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
    /// <param name="position">The object's position</param>
    /// <param name="type">The object's type</param>
    public LevelObjectData(Vector2 position, LevelObjectType type)
    {
        this.x = position.x;
        this.y = position.y;
        this.type = type;
    }

    #endregion
}
