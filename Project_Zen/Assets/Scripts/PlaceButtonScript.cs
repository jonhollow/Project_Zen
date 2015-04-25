using UnityEngine;
using System.Collections;

/// <summary>
/// Script that controls the object placement buttons
/// </summary>
public class PlaceButtonScript : MonoBehaviour
{
    #region Fields

    public LevelObjectType objectType;  // The type of object this button creates

    #endregion

    #region Public Methods
    
    /// <summary>
    /// Activates the placer object
    /// </summary>
    public void ActivatePlacerObject()
    {
        // Activates the placer object and sets its type
        LevelEditorScript.PlacerObject.SetActive(true);
        LevelEditorScript.PlacerObject.GetComponent<PlacerObjectScript>().CurrentType = objectType;
    }

    #endregion
}
