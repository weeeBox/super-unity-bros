using UnityEngine;

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
    public const int CELL_TUBE_TOP1 = 5;
    public const int CELL_TUBE_TOP2 = 6;
    public const int CELL_TUBE1     = 7;
    public const int CELL_TUBE2     = 8;

    [HideInInspector]
    public Cell[,] m_Cells;

    [SerializeField]
    private Sprite[] m_Sprites;

    private TileMap m_TileMap;

    [SerializeField]
    int m_Rows;

    [SerializeField]
    int m_Cols;

    float m_Width;
    float m_Height;

    protected override void OnStart()
    {
        m_TileMap = GetComponent<TileMap>();
        GenerateCells();
    }

    void GenerateCells()
    {
        int rows = this.rows;
        int cols = this.cols;

        m_Cells = new Cell[rows, cols];
        m_Width = cols * Constants.CELL_WIDTH;
        m_Height = rows * Constants.CELL_HEIGHT;

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                Sprite tile = m_TileMap.GetTile(j, i);
                if (tile != null)
                {
                    Cell cell = null;

                    if (tile == m_Sprites[CELL_GROUND] ||
                        tile == m_Sprites[CELL_SOLID] ||
                        tile == m_Sprites[CELL_BLANK] ||
                        tile == m_Sprites[CELL_TUBE1] ||
                        tile == m_Sprites[CELL_TUBE2] ||
                        tile == m_Sprites[CELL_TUBE_TOP1] ||
                        tile == m_Sprites[CELL_TUBE_TOP2])
                    {
                        cell = new Cell(this, i, j);
                    }
                    else if (tile == m_Sprites[CELL_BRICK])
                    {
                        cell = new BrickCell(this, i, j);
                    }
                    else if (tile == m_Sprites[CELL_QUESTION])
                    {
                        cell = new CoinsCell(this, i, j, 1);
                    }

                    m_Cells[i, j] = cell;
                }
            }
        }
    }

    public void SetTile(int i, int j, int type)
    {
        m_TileMap.SetTile(new IntVector2(j, i), m_Sprites[type]);
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

        Vector3 initialPosition = m_TileMap.GetPosition(j, i);
        Vector3 position = initialPosition;

        float velocity = JUMP_VELOCITY;

        while (velocity > -JUMP_VELOCITY)
        {
            position.y += velocity * Time.fixedDeltaTime;
            velocity += Constants.GRAVITY * Time.fixedDeltaTime;
            m_TileMap.SetPosition(j, i, position);

            yield return null;
        }

        m_TileMap.SetPosition(j, i, initialPosition);

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
        return i >= 0 && i < m_Rows && j >= 0 && j < m_Cols ? m_Cells[i, j] : null;
    }

    #region Properties

    public Sprite[] sprites { get { return m_Sprites; } }

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

    #endregion
}