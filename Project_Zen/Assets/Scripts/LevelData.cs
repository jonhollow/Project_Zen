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
public class LevelData
{
    #region Fields

    Dictionary<string, LevelObjectData> objects;    // Dictionary of the level's objects by ID

    #endregion

    #region Properties

    /// <summary>
    /// Gets the dictionary of objects in the level, indexed by ID
    /// </summary>
    public Dictionary<string, LevelObjectData> Objects
    { get { return objects; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public LevelData()
    {
        objects = new Dictionary<string, LevelObjectData>();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Makes a copy of the level data
    /// </summary>
    /// <returns>the copy</returns>
    public LevelData Clone()
    {
        // Makes a new level data
        LevelData copyData = new LevelData();

        // Copies over each item of data
        foreach (KeyValuePair<string, LevelObjectData> data in objects)
        {
            copyData.objects.Add(data.Key, new LevelObjectData(data.Value.Position, data.Value.Type));
        }

        return copyData;
    }

    #endregion
}
