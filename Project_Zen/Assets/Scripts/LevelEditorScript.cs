using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls the overall level editor - may change significantly over time
/// </summary>
public class LevelEditorScript : MonoBehaviour
{
    #region Fields

    public GameObject block;    // Prefab for the generic block object

    LevelData levelData;    // The serializable level data

    Dictionary<LevelObjectType, GameObject> objectPrefabs;  // Dictionary of the level object prefabs

    List<GameObject> levelObjects;  // The list of objects in the level

    #endregion

    #region Private Methods

    /// <summary>
	/// Start is called once on object creation
	/// </summary>
	private void Start() 
    {
        // Initializes fields
        levelData = new LevelData();
        levelObjects = new List<GameObject>();
        objectPrefabs = new Dictionary<LevelObjectType, GameObject>();

        // Adds blocks to the object prefab dictionary
        objectPrefabs.Add(LevelObjectType.Block, block);
	}
	
	/// <summary>
    /// Update is called once per frame
	/// </summary>
	private void Update() 
    {
        // Checks for load level on T and save on R -- temporary
        if (Input.GetKeyDown(KeyCode.T))
        {
            Load("testSave");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Save("testSave");
        }
    }

    /// <summary>
    /// OnMouseDown is called when the mouse clicks this object's collider
    /// NOTE: Probably not actually going to keep this method, just nice for testing
    /// </summary>
    private void OnMouseDown()
    {
        // Gets the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Creates a block where the mouse is and adds it to the object list
        levelObjects.Add((GameObject)Instantiate(block, mousePosition, transform.rotation));

        // Adds the block to the level data
        levelData.Objects.Add(new LevelObjectData(mousePosition, LevelObjectType.Block));
    }

    /// <summary>
    /// Saves the level to the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    private void Save(string filename)
    {
        Serializer.SerializeObject("testSave", levelData);
    }

    /// <summary>
    /// Loads the level from the given file
    /// </summary>
    /// <param name="filename">the file name</param>
    private void Load(string filename)
    {
        // Reloads the level data
        levelData = (LevelData)Serializer.DeserializeObject("testSave");

        // Destroys everything currently in the scene
        for (int i = levelObjects.Count - 1; i >= 0; i--)
        {
            Destroy(levelObjects[i]);
            levelObjects.RemoveAt(i);
        }

        // Reconstructs the scene
        foreach (LevelObjectData levelObject in levelData.Objects)
        {
            levelObjects.Add((GameObject)Instantiate(objectPrefabs[levelObject.Type], levelObject.Position, transform.rotation));
        }
    }

    #endregion
}
