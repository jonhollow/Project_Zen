using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Class that holds adjacency info for a tile corner, for use as a dictionary key
/// </summary>
[Serializable]
public class CornerAdjacency : IEquatable<CornerAdjacency>
{
    #region Fields

    bool prev, opp, next;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="prev">previous side adacency</param>
    /// <param name="curr">opposite corner adjacency</param>
    /// <param name="next">next side adacency</param>
    public CornerAdjacency(bool prev, bool opp, bool next)
    {
        this.prev = prev;
        this.opp = opp;
        this.next = next;
    }

    #endregion

    #region Overloads

    /// <summary>
    /// == operator
    /// </summary>
    /// <param name="a">the first object</param>
    /// <param name="b">the second object</param>
    /// <returns>whether or not the objects are equal</returns>
    public static bool operator ==(CornerAdjacency a, CornerAdjacency b)
    {
        return a.prev == b.prev && a.opp == b.opp && a.next == b.next;
    }

    /// <summary>
    /// != operator
    /// </summary>
    /// <param name="a">the first object</param>
    /// <param name="b">the second object</param>
    /// <returns>whether or not the objects are not equal</returns>
    public static bool operator !=(CornerAdjacency a, CornerAdjacency b)
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
        { return this == (CornerAdjacency)o; }
        catch
        { return false; }
    }

    /// <summary>
    /// Equals
    /// </summary>
    /// <param name="o">the other object</param>
    /// <returns>whether or not the objects are equal</returns>
    public bool Equals(CornerAdjacency other)
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
        int code = 0;
        if (prev) { code += 4; }
        if (opp) { code += 2; }
        if (next) { code += 1; }
        return code;
    }

    #endregion
}
