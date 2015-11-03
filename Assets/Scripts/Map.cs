using UnityEngine;

using System.Collections;
using System.IO;

using LunarCore;

public class Map : BaseBehaviour
{
    [HideInInspector]
    public Cell[,] m_Cells;

    [SerializeField]
    private Sprite[] m_Sprites;

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
                int index = map[i, j];
                if (index != -1)
                {
                    m_TileMap.SetTile(new IntVector2(j, i), m_Sprites[index]);
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

    private int[,] ReadMap(string path, out int rows, out int cols)
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