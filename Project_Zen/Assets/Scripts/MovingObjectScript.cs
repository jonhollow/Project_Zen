using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the moving object
/// </summary>
public class MovingObjectScript : GridDragObjectScript
{
    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
	protected override void Start() 
    {
        // Sets the reference to this object
        moving = gameObject;

        base.Start();
	}

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    protected override void UpdatePreviewBlocks()
    {
        
    }

    #endregion
}
