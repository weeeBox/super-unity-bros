using UnityEngine;
using System.Collections;

using LunarCore;

public class Cell
{
    public readonly float x;
    public readonly float y;

    public Cell(float x, float y)
    {
        this.x = x;
        this.y = y;
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