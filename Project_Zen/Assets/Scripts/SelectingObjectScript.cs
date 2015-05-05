using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the selecting object
/// </summary>
public class SelectingObjectScript : GridDragObjectScript
{
    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
	protected override void Start() 
    {
        // Sets the reference to this object
        selecting = gameObject;

        base.Start();
	}

    /// <summary>
    /// Updates the preview blocks grid
    /// </summary>
    protected override void UpdatePreviewBlocks()
    {
        AddAndRemovePreviewBlocks(false);
    }

    #endregion
}
