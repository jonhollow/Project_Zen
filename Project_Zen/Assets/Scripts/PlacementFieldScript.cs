using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script that controls the placement field
/// </summary>
public class PlacementFieldScript : MonoBehaviour
{
    #region Fields

    public Toggle[] placementOptions;   // Array of the placement toggles

    #endregion

    #region Private Methods

    /// <summary>
    /// OnMouseDown is called when the mouse clicks on this object's collider
    /// </summary>
    private void OnMouseDown()
    {
        // Checks for not paused
        if (!GameController.Instance.Paused)
        {
            // Checks which placement option is selected
            foreach (Toggle toggle in placementOptions)
            {
                if (toggle.isOn)
                {
                    toggle.gameObject.GetComponent<PlaceButtonScript>().ActivatePlacerObject();
                    break;
                }
            }
        }
    }

    #endregion
}
