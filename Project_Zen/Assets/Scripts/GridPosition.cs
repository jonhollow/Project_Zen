using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Class that holds a grid position
/// </summary>
[Serializable]
public class GridPosition : IEquatable<GridPosition>
{
    #region Fields

    int row;    // The row
    int column; // The column

    #endregion

    #region Properties

    /// <summary>
    /// Gets the column
    /// </summary>
    public int Column
    { get { return column; } }

    /// <summary>
    /// Gets the row
    /// </summary>
    public int Row
    { get { return row; } }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="row">the row</param>
    /// <param name="column">the column</param>
    public GridPosition(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Converts the grid coordinates to world coordinates
    /// </summary>
    /// <returns>the world location</returns>
    public Vector2 ToWorldPosition()
    {
        return new Vector2((column * Constants.GRID_CELL_SIZE) + Constants.GRID_X_OFFSET, (row * Constants.GRID_CELL_SIZE) + Constants.GRID_Y_OFFSET);
    }

    #endregion

    #region Standard Overloads

    /// <summary>
    /// Returns a nicely formatted grid position string
    /// </summary>
    /// <returns>the string</returns>
    public override string ToString() 
    {
        return "(" + row + ", " + column + ")";
    }

    /// <summary>
    /// == operator
    /// </summary>
    /// <param name="a">the first object</param>
    /// <param name="b">the second object</param>
    /// <returns>whether or not the objects are equal</returns>
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.row == b.row && a.column == b.column;
    }

    /// <summary>
    /// != operator
    /// </summary>
    /// <param name="a">the first object</param>
    /// <param name="b">the second object</param>
    /// <returns>whether or not the objects are not equal</returns>
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="o">the other object</param>
    /// <returns>whether or not the objects are equal</returns>
    public override bool Equals(object o)
    {
        try
        { return this == (GridPosition)o; }
        catch
        { return false; }
    }

    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="o">the other object</param>
    /// <returns>whether or not the objects are equal</returns>
    public bool Equals(GridPosition other)
    {
        try
        { return this == other; }
        catch
        { return false; }
    }

    /// <summary>
    /// GetHashCode
    /// </summary>
    /// <returns>the hash code</returns>
    public override int GetHashCode()
    {
        return (row * 100) + column;
    }

    #endregion
}
