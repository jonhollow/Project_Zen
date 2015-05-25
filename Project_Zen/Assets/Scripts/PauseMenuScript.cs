using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script that controls pause menus
/// </summary>
public class PauseMenuScript : MonoBehaviour
{
    #region Fields

    

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the cancel button being pressed
    /// </summary>
    public void CancelButtonPressed()
    {
        // Hides the load menu
        gameObject.SetActive(false);
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Pauses the game on enable
    /// </summary>
    protected virtual void OnEnable()
    {
        LevelController.Instance.Paused = true;
    }

    /// <summary>
    /// Unpauses the game on disable
    /// </summary>
    protected virtual void OnDisable()
    {
        LevelController.Instance.Paused = false;
    }

    #endregion
}
