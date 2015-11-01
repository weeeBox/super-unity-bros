using UnityEngine;
using System.Collections;

using LunarCore;

public interface IMapCollider
{
    void OnCollision(Cell cell);
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
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
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

        float cy = 0.5f * Constants.CELL_HEIGHT;
        for (int i = 0; i < rows; ++i)
        {
            float cx = 0.5f * Constants.CELL_WIDTH;
            for (int j = 0; j < cols; ++j)
            {
                int index = (rows - 1 - i) * cols + j;
                int type = data[index];
                if (type != 0)
                {
                    m_Cells[i, j] = new Cell(cx, cy);
                }
                cx += Constants.CELL_WIDTH;
            }
            cy += Constants.CELL_HEIGHT;
        }
    }

    public void HandleCollisions(IMapCollider collider)
    {
        Rect rect = collider.colliderRect;

        Cell cell;

        cell = GetCell(rect.xMin, rect.yMax);
        if (cell != null) collider.OnCollision(cell);

        cell = GetCell(rect.xMax, rect.yMax);
        if (cell != null) collider.OnCollision(cell);

        cell = GetCell(rect.xMin, rect.yMin);
        if (cell != null) collider.OnCollision(cell);
        
        cell = GetCell(rect.xMax, rect.yMin);
        if (cell != null) collider.OnCollision(cell);
    }

    public Cell GetCell(float x, float y)
    {
        int cx = (int) (x * Constants.CELL_WIDTH_INV);
        int cy = (int) (y * Constants.CELL_HEIGHT_INV);

        return cx >= 0 && cx < m_Cols && cy >= 0 && cy < m_Cols ? m_Cells[cy, cx] : null;
    }
}