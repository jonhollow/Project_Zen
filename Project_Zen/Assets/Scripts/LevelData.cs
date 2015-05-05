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

    LevelObjectData[,] objectsGrid;

    #endregion

    #region Properties

    /// <summary>
    /// Gets the object grid
    /// </summary>
    public LevelObjectData[,] Grid
    { get { return objectsGrid; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    public LevelData()
    {
        objectsGrid = new LevelObjectData[Constants.GRID_ROWS, Constants.GRID_COLUMNS];
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Clears the level data
    /// </summary>
    public void Clear()
    {
        // Sets each item in the grid to null
        for (int i = 0; i < objectsGrid.GetLength(0); i++)
        {
            for (int j = 0; j < objectsGrid.GetLength(1); j++)
            { objectsGrid[i, j] = null; }
        }
    }

    /// <summary>
    /// Makes a copy of the level data
    /// </summary>
    /// <returns>the copy</returns>
    public LevelData Clone()
    {
        // Makes a new level data
        LevelData copyData = new LevelData();

        // Copies over the array
        for (int i = 0; i < objectsGrid.GetLength(0); i++)
        {
            for (int j = 0; j < objectsGrid.GetLength(1); j++)
            {
                if (objectsGrid[i, j] != null)
                { copyData.objectsGrid[i, j] = objectsGrid[i, j].Clone(); }
            }
        }

        return copyData;
    }

    #endregion
}
