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
        return Vector3.zero;
    }

    public static void SetPosition(this Tilemap tilemap, int x, int y, Vector3 position)
    {
    }
}
