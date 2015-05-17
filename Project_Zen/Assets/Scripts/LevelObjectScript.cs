using UnityEngine;
using System.Collections;

/// <summary>
/// Parent script for level objects
/// </summary>
public class LevelObjectScript : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    /// Resets the tiling of the object
    /// Defined here so that it can be called for all objects; overridden in objects that actually tile
    /// </summary>
    public virtual void Retile()
    {
        
    }

    #endregion
}
