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

        if (GUILayout.Button("Generate tilemap"))
        {
            target.GenerateTilemap();
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

        for (int i = 0; i < map.m_Rows; ++i)
        {
            for (int j = 0; j < map.m_Cols; ++j)
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
