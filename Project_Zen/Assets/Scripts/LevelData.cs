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
/// A class for serializing level data
/// </summary>
[Serializable]
public class LevelData
{
    #region Fields

    List<LevelObjectData> objects;  // A list of objects in the level

    #endregion

    #region Properties

    /// <summary>
    /// Gets the list of objects in the level
    /// </summary>
    public List<LevelObjectData> Objects
    { get { return objects; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public LevelData()
    {
        objects = new List<LevelObjectData>();
    }

    #endregion
}
