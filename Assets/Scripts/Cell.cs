using UnityEngine;
using System.Collections;

using LunarCore;

public class Cell
{
    public readonly int i;
    public readonly int j;
    public readonly float x;
    public readonly float y;

    public Cell(int i, int j)
    {
        this.i = i;
        this.j = j;
        this.x = (0.5f + j) * Constants.CELL_WIDTH;
        this.y = (0.5f + i) * Constants.CELL_HEIGHT;
    }

    public Vector3 position
    {
        get { return new Vector3(x, y, 0); }
    }

    public float top
    {
        get { return y + 0.5f * Constants.CELL_HEIGHT; }
    }

    public float bottom
    {
        get { return y - 0.5f * Constants.CELL_HEIGHT; }
    }

    public float left
    {
        get { return x - 0.5f * Constants.CELL_WIDTH; }
    }

    public float right
    {
        get { return x + 0.5f * Constants.CELL_WIDTH; }
    }
}