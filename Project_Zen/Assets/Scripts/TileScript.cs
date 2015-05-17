using UnityEngine;
using System.Collections;

/// <summary>
/// Script that controls tiling objects
/// </summary>
public class TileScript : LevelObjectScript
{
    #region Fields

    [SerializeField]SpriteRenderer[] edges; // Array of the tile's edges, from top going clockwise

    #endregion
    
    #region Public Methods

    /// <summary>
    /// Resets the tiling of the object
    /// </summary>
    public override void Retile()
    {
        // Gets this object's grid position
        GridPosition gridPos = GameController.WorldToGrid(transform.position);

        // Gets in which positions the tile has adjacent tiles
        bool[] adjacencies = new bool[] { gridPos.Row + 1 < Constants.GRID_ROWS && GameController.Instance.LevelGrid[gridPos.Row + 1, gridPos.Column] != null,
            gridPos.Row + 1 < Constants.GRID_ROWS && gridPos.Column + 1 < Constants.GRID_COLUMNS && GameController.Instance.LevelGrid[gridPos.Row + 1, gridPos.Column + 1] != null, 
            gridPos.Column + 1 < Constants.GRID_COLUMNS && GameController.Instance.LevelGrid[gridPos.Row, gridPos.Column + 1] != null, 
            gridPos.Row - 1 >= 0 && gridPos.Column + 1 < Constants.GRID_COLUMNS && GameController.Instance.LevelGrid[gridPos.Row - 1, gridPos.Column + 1] != null,
            gridPos.Row - 1 >= 0 && GameController.Instance.LevelGrid[gridPos.Row - 1, gridPos.Column] != null,
            gridPos.Row - 1 >= 0 && gridPos.Column - 1 >= 0 && GameController.Instance.LevelGrid[gridPos.Row - 1, gridPos.Column - 1] != null,
            gridPos.Column - 1 >= 0 && GameController.Instance.LevelGrid[gridPos.Row, gridPos.Column - 1] != null,
            gridPos.Row + 1 < Constants.GRID_ROWS && gridPos.Column - 1 >= 0 && GameController.Instance.LevelGrid[gridPos.Row + 1, gridPos.Column - 1] != null };

        // Loops through the sides
        for (int i = 0; i < edges.Length; i += 2)
        { edges[i].sprite = GameController.Instance.BlockTileSideSprites[adjacencies[i]]; }

        // Loops through the corners
        for (int i = 1; i < edges.Length; i += 2)
        {
            int prev = i - 1;
            int next = i + 1;
            if (prev < 0) { prev = edges.Length - 1; }
            if (next == edges.Length) { next = 0; }
            edges[i].sprite = GameController.Instance.BlockTileCornerSprites[new CornerAdjacency(adjacencies[prev], adjacencies[i], adjacencies[next])];
        }
    }

    #endregion
}
