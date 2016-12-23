using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtensions
{
    public static Tile GetTile(this Tilemap tilemap, int x, int y)
    {
        return tilemap.GetTile<Tile>(new Vector3Int(x, y, 0));
    }

    public static void SetTile(this Tilemap tilemap, int x, int y, Tile tile)
    {
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public static Vector3 GetPosition(this Tilemap tilemap, int x, int y)
    {
        var tile = tilemap.GetTile(x, y);
        var cellSize = tilemap.cellSize;
        return new Vector3(cellSize.x * x, cellSize.y * y, 0);
    }

    public static void SetOffset(this Tilemap tilemap, int x, int y, Vector3 position)
    {
        var tile = tilemap.GetTile(x, y);
        tile.transform.SetTRS(position, Quaternion.identity, Vector3.one);
        tilemap.RefreshTile(new Vector3Int(x, y, 0));
    }
}
