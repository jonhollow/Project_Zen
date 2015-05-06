using UnityEngine;
using System.Collections;

/// <summary>
/// Script that controls a player start position object
/// </summary>
public class PlayerStartPositionScript : MonoBehaviour
{
    #region Private Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    private void Start()
    {
        // Spawns a player if in an actual level
        if (!GameController.Instance.InLevelEditor)
        {
            Instantiate(GameController.Instance.ObjectPrefabs[LevelObjectType.Player], transform.position, transform.rotation);
            GameController.Instance.DestroyLevelObject(GameController.WorldToGrid(transform.position));
        }
    }

    #endregion
}
