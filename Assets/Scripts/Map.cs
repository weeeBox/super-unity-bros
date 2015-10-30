using UnityEngine;
using System.Collections;

using LunarCore;

public class Map : BaseBehaviour
{
    private static readonly int[] TEMP_DATA = {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
    };

    private const int MAP_ROWS = 14;
    private const int MAP_COLS = 16;

    public Cell[,] cells;

    protected override void OnStart()
    {
        cells = new Cell[MAP_ROWS, MAP_COLS];

        float cy = 0.5f * Constants.CELL_HEIGHT;
        for (int i = 0; i < MAP_ROWS; ++i)
        {
            float cx = 0.5f * Constants.CELL_WIDTH;
            for (int j = 0; j < MAP_COLS; ++j)
            {
                int index = (MAP_ROWS - 1 - i) * MAP_COLS + j;
                int type = TEMP_DATA[index];
                if (type != 0)
                {
                    cells[i, j] = new Cell(cx, cy);
                }
                cx += Constants.CELL_WIDTH;
            }
            cy += Constants.CELL_HEIGHT;
        }
    }

    public Cell GetCell(float x, float y)
    {
        int cx = (int) (x * Constants.CELL_WIDTH_INV);
        int cy = (int) (y * Constants.CELL_HEIGHT_INV);

        return cx >= 0 && cx < MAP_COLS && cy >= 0 && cy < MAP_ROWS ? cells[cy, cx] : null;
    }
}