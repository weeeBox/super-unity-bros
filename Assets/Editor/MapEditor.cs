using UnityEngine;
using UnityEditor;
using System.Collections;

using LunarCore;

[CustomEditor(typeof(Map))]
public class MapEditor : BaseEditor<Map>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load map..."))
        {
            MapInfo map = MapLoader.LoadMap();
            if (map != null)
            {
                GenerateMap(map);
            }
        }
    }

    void GenerateMap(MapInfo map)
    {
        int rows = map.rows;
        int cols = map.cols;

        TileMap tileMap = target.GetComponent<TileMap>();
        target.rows = rows;
        target.cols = cols;
        
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                int type = map[i, j];
                if (type != -1)
                {
                    tileMap.SetTile(new IntVector2(j, i), target.sprites[type]);
                }
            }
        }
    }

    protected override void OnSceneGUI()
    {
        Map map = target;
        Cell[,] cells = map.m_Cells;

        if (cells == null)
            return;

        float x = map.transform.position.x - 0.5f * Constants.CELL_WIDTH;
        float y = map.transform.position.y - 0.5f * Constants.CELL_HEIGHT;

        for (int i = 0; i < map.rows; ++i)
        {
            for (int j = 0; j < map.cols; ++j)
            {
                Cell cell = cells [i, j];
                if (cell != null)
                {
                    DebugEx.DrawRect(x + cell.x, y + cell.y, Constants.CELL_WIDTH, Constants.CELL_HEIGHT);
                }
            }
        }
    }
}
