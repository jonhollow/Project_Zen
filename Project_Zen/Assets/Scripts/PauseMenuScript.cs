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

    #region Protected Methods

    /// <summary>
    /// Pauses the game on enable
    /// </summary>
    protected virtual void OnEnable()
    {
        GameController.Instance.Paused = true;
    }

    /// <summary>
    /// Unpauses the game on disable
    /// </summary>
    protected virtual void OnDisable()
    {
        GameController.Instance.Paused = false;
    }

    #endregion
}
