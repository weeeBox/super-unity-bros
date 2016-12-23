using UnityEngine;
using UnityEngine.Tilemaps;

using System.Collections;
using System.IO;

using LunarCore;
using Action = System.Action;

public class Map : BaseBehaviour
{
    public const int CELL_NONE = -1;

    public const int CELL_GROUND    = 0;
    public const int CELL_SOLID     = 1;
    public const int CELL_BLANK     = 2;
    public const int CELL_QUESTION  = 3;
    public const int CELL_BRICK     = 4;
    public const int CELL_BRICK_2   = 26;
    public const int CELL_TUBE_TOP1 = 5;
    public const int CELL_TUBE_TOP2 = 6;
    public const int CELL_TUBE1     = 7;
    public const int CELL_TUBE2     = 8;

    [HideInInspector]
    public Cell[,] m_cells;

    [SerializeField]
    private Tile[] m_tiles;

    private Tilemap m_tileMap;

    [SerializeField]
    int m_Rows;

    [SerializeField]
    int m_Cols;

    float m_Width;
    float m_Height;

    protected override void OnStart()
    {
        m_tileMap = GetComponent<Tilemap>();
        GenerateCells();
    }

    void GenerateCells()
    {
        int rows = this.rows;
        int cols = this.cols;

        m_cells = new Cell[rows, cols];
        m_Width = cols * Constants.CELL_WIDTH;
        m_Height = rows * Constants.CELL_HEIGHT;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                var tile = m_tileMap.GetTile(j, i);
                if (tile != null)
                {
                    Cell cell = null;

                    if (tile == m_tiles[CELL_GROUND] ||
                        tile == m_tiles[CELL_SOLID] ||
                        tile == m_tiles[CELL_BLANK] ||
                        tile == m_tiles[CELL_TUBE1] ||
                        tile == m_tiles[CELL_TUBE2] ||
                        tile == m_tiles[CELL_TUBE_TOP1] ||
                        tile == m_tiles[CELL_TUBE_TOP2])
                    {
                        cell = new Cell(this, i, j);
                    }
                    else if (tile is BrickTile)
                    {
                        cell = new BrickCell(this, i, j);
                    }
                    else if (tile is QuestionTile)
                    {
                        var questionTile = tile as QuestionTile;
                        cell = new CoinsCell(this, i, j, 1);
                    }

                    m_cells[i, j] = cell;
                }
            }
        }
    }

    public void SetTile(int i, int j, int type)
    {
        m_tileMap.SetTile(j, i, m_tiles[type]);
    }

    public void RemoveTile(int i, int j)
    {
        m_tileMap.SetTile(j, i, null);
        m_cells[i, j] = null;
    }

    public void Jump(int i, int j, Action finishAction = null)
    {
        Cell cell = GetCellAt(i, j);
        assert.IsNotNull(cell);

        if (cell != null)
        {
            StartCoroutine(JumpCoroutine(i, j, finishAction));
        }
    }

    IEnumerator JumpCoroutine(int i, int j, Action finishAction)
    {
        const float JUMP_VELOCITY = 30f;

        Vector3 offset = Vector3.zero;
        float velocity = JUMP_VELOCITY;
        while (velocity > -JUMP_VELOCITY)
        {
            offset.y += velocity * Time.fixedDeltaTime;
            velocity += Constants.GRAVITY * Time.fixedDeltaTime;
            m_tileMap.SetOffset(j, i, offset);

            yield return null;
        }

        m_tileMap.SetOffset(j, i, Vector3.zero);

        if (finishAction != null)
        {
            finishAction();
        }
    }

    public Cell GetCell(float x, float y)
    {
        if (x >= 0 && x <= m_Width && y >= 0 && y <= m_Height)
        {
            int i = (int) (y * Constants.CELL_HEIGHT_INV);
            int j = (int) (x * Constants.CELL_WIDTH_INV);

            return GetCellAt(i, j);
        }

        return null;
    }

    public Cell GetCellAt(int i, int j)
    {
        return i >= 0 && i < m_Rows && j >= 0 && j < m_Cols ? m_cells[i, j] : null;
    }

    #region Properties

    public Tile[] tiles { get { return m_tiles; } }

    public int rows
    {
        get { return m_Rows; }

        #if UNITY_EDITOR
        set { m_Rows = value; }
        #endif
    }

    public int cols
    {
        get { return m_Cols; }

        #if UNITY_EDITOR
        set { m_Cols = value; }
        #endif
    }

    public float width { get { return m_Width; } }
    public float height { get { return m_Height; } }

    #endregion
}