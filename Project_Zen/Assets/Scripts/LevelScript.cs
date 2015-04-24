using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Parent script that controls the overall level - may change significantly over time
/// </summary>
public class LevelScript : MonoBehaviour
{
    #region Fields

    public GameObject block;    // Prefab for the generic block object

    protected LevelData levelData;  // The serializable level data

    protected List<GameObject> levelObjects;    // The list of objects in the level

    Dictionary<LevelObjectType, GameObject> objectPrefabs;  // Dictionary of the level object prefabs

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads the level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    public virtual void Load(string filename)
    {
        // Reloads the level data if possible
        LevelData loadedData = (LevelData)Serializer.DeserializeObject(filename);
        if (loadedData != null)
        {
            levelData = loadedData;
            RestoreToLevelData();
        }
    }

    #endregion

    #region Protected Methods

    /// <summary>
    /// Start is called once on object creation
    /// </summary>
    protected virtual void Start()
    {
        // Initializes fields
        levelData = new LevelData();
        levelObjects = new List<GameObject>();
        objectPrefabs = new Dictionary<LevelObjectType, GameObject>();

        // Adds blocks to the object prefab dictionary
        objectPrefabs.Add(LevelObjectType.Block, block);
    }

    /// <summary>
    /// Restores the level to the level data
    /// </summary>
    protected void RestoreToLevelData()
    {
        // Destroys everything currently in the scene
        while (levelObjects.Count > 0)
        {
            Destroy(levelObjects[0]);
            levelObjects.RemoveAt(0);
        }

        // Reconstructs the scene
        foreach (KeyValuePair<string, LevelObjectData> data in levelData.Objects)
        {
            // Creates the object and gives it its ID
            levelObjects.Add((GameObject)Instantiate(objectPrefabs[data.Value.Type], data.Value.Position, transform.rotation));
            levelObjects[levelObjects.Count - 1].GetComponent<LevelObjectScript>().ID = data.Key;
        }
    }

    #endregion
}
