using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtensions
{
    public static Sprite GetTile(this Tilemap tilemap, int x, int y)
    {
        return tilemap.GetTile<Tile>(new Vector3Int(x, y, 0)).sprite;
    }

    public static void SetTile(this Tilemap tilemap, int x, int y, Sprite sprite)
    {
        var tile = new Tile();
        tile.sprite = sprite;
        tilemap.SetTile(new Vector3Int(x, y, 0), tile);
    }

    public static Vector3 GetPosition(this Tilemap tilemap, int x, int y)
    {
        var rect = tilemap.GetTile(x, y).rect;
        return new Vector3(rect.x, rect.y, 0);
    }

    public static void SetPosition(this Tilemap tilemap, int x, int y, Vector3 position)
    {
    }
}
