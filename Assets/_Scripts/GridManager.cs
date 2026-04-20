using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("Grid Settings")]
    [SerializeField] private int totalCols = 9;
    [SerializeField] private int totalRows = 5;

    private Dictionary<Vector2Int, Tiles> grid = new();

    void Awake()
    {
        Instance = this;
        RegisterAllTiles();
    }

    void RegisterAllTiles()
    {
        foreach (var tile in FindObjectsByType<Tiles>(FindObjectsSortMode.None))
        {
            var key = new Vector2Int(tile.col, tile.row);
            if (!grid.ContainsKey(key))
                grid[key] = tile;
        }
    }

    public Tiles GetTileAtWorldPos(Vector2 worldPos)
    {
        Tiles closest = null;
        float minDist = float.MaxValue;

        foreach (var tile in grid.Values)
        {
            float dist = Vector2.Distance(tile.transform.position, worldPos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = tile;
            }
        }
        return closest;
    }

    public bool CanPlace(Tiles originTile, Vector2Int size)
    {
        for (int c = 0; c < size.x; c++)
        {
            for (int r = 0; r < size.y; r++)
            {
                var key = new Vector2Int(originTile.col + c, originTile.row + r);
                if (!grid.TryGetValue(key, out var tile) || tile.IsOccupied)
                    return false;
            }
        }
        return true;
    }

    public List<Tiles> LockArea(Tiles originTile, Vector2Int size, int objectID)
    {
        var lockedTiles = new List<Tiles>();
        for (int c = 0; c < size.x; c++)
        {
            for (int r = 0; r < size.y; r++)
            {
                var key = new Vector2Int(originTile.col + c, originTile.row + r);
                if (grid.TryGetValue(key, out var tile))
                {
                    tile.Lock(objectID);
                    lockedTiles.Add(tile);
                }
            }
        }
        return lockedTiles;
    }

    public void UnlockArea(List<Tiles> tiles)
    {
        foreach (var tile in tiles)
            tile.Unlock();
    }

    public List<Tiles> GetTilesInArea(Tiles originTile, Vector2Int size)
    {
        var result = new List<Tiles>();
        for (int c = 0; c < size.x; c++)
        {
            for (int r = 0; r < size.y; r++)
            {
                var key = new Vector2Int(originTile.col + c, originTile.row + r);
                if (grid.TryGetValue(key, out var tile))
                    result.Add(tile);
            }
        }
        return result;
    }

    public void HideAllTiles()
    {
        foreach (Tiles tile in grid.Values)
        {
            tile.SetVisible(false);
        }
    }

    public void ShowAllTiles()
    {
        foreach (Tiles tile in grid.Values)
        {
            tile.SetVisible(true);
        }
    }
}