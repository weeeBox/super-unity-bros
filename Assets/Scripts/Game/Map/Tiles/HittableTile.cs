using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

public enum HittableTileType
{
    Empty,
    Coins,
    Mushroom,
    Star,
    Life
}

public class HittableTile : Tile
{
    [SerializeField]
    private HittableTileType m_type = HittableTileType.Empty;

    [SerializeField]
    private int m_coinCount = 0;

    void OnEnable()
    {
        this.flags = TileFlags.LockTransform;
    }

    public HittableTileType type
    {
        get { return m_type; }
        set { m_type = value; }
    }

    public int cointCount
    {
        get { return m_coinCount; }
        set { m_coinCount = value; }
    }
}
