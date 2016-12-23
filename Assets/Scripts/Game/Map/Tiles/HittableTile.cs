using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public class HittableTile : Tile
{
    void OnEnable()
    {
        this.flags = TileFlags.LockTransform;
    }
}
