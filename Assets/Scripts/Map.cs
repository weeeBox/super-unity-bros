using UnityEngine;

using System.Collections;
using System.IO;

using LunarCore;
using Action = System.Action;

[System.Serializable]
public class MapPrefabs
{
    public GameObject questionPrefab;
}

public class Map : BaseBehaviour
{
    public const int CELL_NONE = -1;
    public const int CELL_GROUND = 0;
    public const int CELL_BRICK = 1;
    public const int CELL_QUESTION = 2;
    public const int CELL_BLANK = 3;
    public const int CELL_SOLID = 4;

    [HideInInspector]
    public Cell[,] m_Cells;

    [SerializeField]
    private Sprite[] m_Sprites;

    [SerializeField]
    private MapPrefabs m_Prefabs;

    private TileMap m_TileMap;

    [HideInInspector]
    public int m_Rows;

    [HideInInspector]
    public int m_Cols;

    protected override void OnStart()
    {
        m_TileMap = GetComponent<TileMap>();
        GenerateTilemap();
    }

    public void GenerateTilemap()
    {
        int rows, cols;
        int[,] map = ReadMap("world1-1", out rows, out cols);
        m_Rows = rows;
        m_Cols = cols;
        m_Cells = new Cell[rows, cols];

        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                int type = map[i, j];
                if (type != CELL_NONE)
                {
                    SetCell(i, j, type);
                }
            }
        }
    }

    public void SetCell(int i, int j, int type)
    {
        if (type != CELL_NONE)
        {
            m_TileMap.SetTile(new IntVector2(j, i), m_Sprites[type]);
            m_Cells[i, j] = CreateCell(type, i, j);
        }
        else
        {
            assert.IsNotNull(m_Cells[i, j]);

            m_Cells[i, j] = null;
            m_TileMap.SetTile(i, j, null);
        }
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

    Cell CreateCell(int type, int i, int j)
    {
        switch (type)
        {
            case CELL_BRICK:
                return new BrickCell(this, i, j);

            case CELL_QUESTION:
                return new CoinsCell(this, i, j, 1);
        }

        return new Cell(this, i, j);
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

    int[,] ReadMap(string path, out int rows, out int cols)
    {
        Object obj = Resources.Load("Maps/world1-1");
        TextAsset data = obj as TextAsset;
        assert.IsNotNull(data);

        using (StringReader reader = new StringReader(data.text))
        {
            string[] tokens = reader.ReadLine().Split(' ');
            rows = int.Parse(tokens[0]);
            cols = int.Parse(tokens[1]);

            int[,] map = new int[rows, cols];

            for (int i = 0; i < rows; ++i)
            {
                tokens = reader.ReadLine().Split(' ');
                for (int j = 0; j < cols; ++j)
                {
                    map[rows - 1 - i, j] = int.Parse(tokens[j]) - 1;
                }
            }

            return map;
        }
    }
}