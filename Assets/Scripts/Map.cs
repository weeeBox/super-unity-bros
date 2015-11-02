using UnityEngine;
using System.Collections;

using LunarCore;

public interface IMapCollider
{
    bool OnCollision(Cell cell);
    Rect colliderRect { get; }
}

public class Map : BaseBehaviour
{
    private static readonly int[] TEMP_DATA =
    {
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
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1,
    };

    private const int MAP_ROWS = 14;
    private const int MAP_COLS = 16;

    [HideInInspector]
    public Cell[,] m_Cells;

    private int m_Rows;
    private int m_Cols;

    protected override void OnStart()
    {
        CreateCells();
    }

    protected virtual void CreateCells()
    {
        CreateCells(TEMP_DATA, MAP_ROWS, MAP_COLS);
    }

    protected void CreateCells(int[] data, int rows, int cols)
    {
        m_Cells = new Cell[rows, cols];
        m_Rows = rows;
        m_Cols = cols;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                int index = (rows - 1 - i) * cols + j;
                int type = data[index];
                if (type != 0)
                {
                    m_Cells[i, j] = new Cell(i, j);
                }
            }
        }
    }

    public Cell GetCell(float x, float y)
    {
        int i = (int) (y * Constants.CELL_HEIGHT_INV);
        int j = (int) (x * Constants.CELL_WIDTH_INV);

        return GetCellAt(i, j);
    }

    public Cell GetCellAt(int i, int j)
    {
        return i >= 0 && i < m_Rows && j >= 0 && j < m_Cols ? m_Cells[i, j] : null;
    }
}