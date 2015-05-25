using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script that controls a pause menu with a files display (save/load)
/// </summary>
[RequireComponent(typeof(ToggleGroup))]
public class PauseMenuWFilesScript : PauseMenuScript
{
    #region Fields

    public Image scrollField;       // The scroll field for the file display
    public Toggle levelFilePrefab;  // The prefab for a level file display option

    [SerializeField]float levelFileTopOffset;   // The Y offset of the top level file display
    [SerializeField]float levelFileLeftOffset;  // The left offset of the level file displays
    [SerializeField]float levelFileRightOffset; // The right offset of the level file displays
    [SerializeField]float levelFileSpacing;     // The Y spacing between the level file displays
    [SerializeField]int levelOptionsPerScreen;  // The number of level options that can fit on the screen at once

    ToggleGroup toggleGroup;        // The toggle group for the file display
    List<Toggle> levelFileOptions;  // The list of level file option toggles

    #endregion

    #region Protected Methods

    /// <summary>
    /// Initializes the menu
    /// </summary>
    protected virtual void Initialize()
    {
        // Initializes fields
        levelFileOptions = new List<Toggle>();
        toggleGroup = GetComponent<ToggleGroup>();
    }

    /// <summary>
    /// Updates the menu on enable
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();

        // Checks for initialize
        if (levelFileOptions == null)
        { Initialize(); }

        // Creates the level file options
        foreach (KeyValuePair<string, LevelData> level in LevelController.Instance.SavedLevels)
        {
            // Creates a new toggle
            Toggle newToggle = Instantiate(levelFilePrefab);

            // Sets the toggle's data
            RectTransform newToggleRect = newToggle.GetComponent<RectTransform>();
            newToggleRect.SetParent(scrollField.GetComponent<RectTransform>());
            newToggleRect.localScale = new Vector3(1, 1, 1);
            newToggle.GetComponentInChildren<Text>().text = level.Key;
            newToggle.group = toggleGroup;
            newToggle.onValueChanged.AddListener(LevelFileValueChanged);

            // Positions the toggle
            float yOffset = levelFileTopOffset + (levelFileSpacing * levelFileOptions.Count);
            newToggleRect.offsetMin = new Vector2(levelFileLeftOffset, yOffset - newToggleRect.sizeDelta.y);
            newToggleRect.offsetMax = new Vector2(levelFileRightOffset, yOffset);

            // Adds the new toggle to the list
            levelFileOptions.Add(newToggle);
        }

        // Updates scroll field height
        scrollField.GetComponent<RectTransform>().offsetMin = new Vector2(0,
            Mathf.Max(0, levelFileOptions.Count - levelOptionsPerScreen) * levelFileSpacing);
    }

    /// <summary>
    /// Updates the menu on disable
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();

        // Destroys the level file options
        for (int i = levelFileOptions.Count - 1; i >= 0; i--)
        {
            Destroy(levelFileOptions[i].gameObject);
            levelFileOptions.RemoveAt(i);
        }
    }

    /// <summary>
    /// Handles clicking on a level file option
    /// </summary>
    /// <param name="value">the new value</param>
    protected virtual void LevelFileValueChanged(bool value)
    {
        // Finds the toggle that was changed and updates the current level name to match it
        foreach (Toggle toggle in levelFileOptions)
        {
            if (toggle.isOn)
            { 
                LevelController.Instance.CurrentLevelName = toggle.GetComponentInChildren<Text>().text;
                break;
            }
        }
    }

    #endregion
}
