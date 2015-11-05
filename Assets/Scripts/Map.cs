using UnityEngine;

using System.Collections;
using System.IO;

using LunarCore;

[System.Serializable]
public class MapPrefabs
{
    public GameObject questionPrefab;
}

public class Map : BaseBehaviour
{
    const int CELL_GROUND = 0;
    const int CELL_BRICK = 1;
    const int CELL_QUESTION = 2;
    const int CELL_QUESTION_EMPTY = 3;
    const int CELL_SOLID = 4;

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
                int index = map[i, j];
                if (index != -1)
                {
                    m_TileMap.SetTile(new IntVector2(j, i), m_Sprites[index]);
                    m_Cells[i, j] = CreateCell(index, i, j);
                }
            }
        }
    }

    Cell CreateCell(int type, int i, int j)
    {
        Cell cell = new Cell(i, j);

        switch (type)
        {
            case CELL_QUESTION:
            {
                GameObject obj = Instantiate(m_Prefabs.questionPrefab) as GameObject;
                obj.transform.parent = transform;
                obj.transform.localPosition = cell.position;
                break;
            }
        }
        return cell;
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