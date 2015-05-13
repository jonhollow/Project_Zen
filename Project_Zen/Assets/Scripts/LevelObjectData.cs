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
/// Class for serializing level data
/// </summary>
[Serializable]
public class LevelObjectData
{
    #region Fields

    LevelObjectType type;

    #endregion

    #region Properties

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
    public LevelObjectData(LevelObjectType type)
    {
        this.type = type;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Makes a copy of the level object
    /// </summary>
    /// <returns>the copy</returns>
    public LevelObjectData Clone()
    {
        // Makes a new level object
        LevelObjectData copy = new LevelObjectData(type);
        return copy;
    }

    #endregion
}
