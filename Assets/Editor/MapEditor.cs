using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
	void OnSceneGUI()
	{
		Map map = target as Map;
		Cell[,] cells = map.cells;

		if (cells == null) return;

		float x = map.transform.position.x - 0.5f * Constants.CELL_WIDTH;
		float y = map.transform.position.y - 0.5f * Constants.CELL_HEIGHT;

		for (int i = 0; i < 14; ++i)
		{
			for (int j = 0; j < 16; ++j)
			{
				Cell cell = cells[i, j];
				if (cell != null)
				{
					DrawRect(x + cell.x, y + cell.y, Constants.CELL_WIDTH, Constants.CELL_HEIGHT);
				}
			}
		}
	}

	void DrawRect(float x, float y, float w, float h)
	{
		Debug.DrawLine(new Vector2(x, y), new Vector2(x + w, y));
		Debug.DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h));
		Debug.DrawLine(new Vector2(x + w, y + h), new Vector2(x, y + h));
		Debug.DrawLine(new Vector2(x, y + h), new Vector2(x, y));
	}
}
