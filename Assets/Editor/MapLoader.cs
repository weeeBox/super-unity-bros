using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

static class MapLoader
{
    public static MapInfo LoadMap()
    {
        string directory = Path.Combine(Application.dataPath, "Editor/Maps");
        string path = EditorUtility.OpenFilePanelWithFilters("Select map", directory, new string[] { "Map txt files", "txt" });
        if (!string.IsNullOrEmpty(path))
        {
            return ReadMap(path);
        }

        return null;
    }

    static MapInfo ReadMap(string path)
    {
        string data = File.ReadAllText(path);
        using (StringReader reader = new StringReader(data))
        {
            string[] tokens = reader.ReadLine().Split(' ');
            int rows = int.Parse(tokens[0]);
            int cols = int.Parse(tokens[1]);

            MapInfo map = new MapInfo(rows, cols);
            
            for (int i = 0; i < rows; ++i)
            {
                tokens = reader.ReadLine().Split(' ');
                for (int j = 0; j < cols; ++j)
                {
                    map[rows - 1 - i, j] = int.Parse(tokens[j]) - 1;
                }
            }

            int enemyCount = int.Parse(reader.ReadLine());

            for (int i = 0; i < enemyCount; ++i)
            {
                tokens = reader.ReadLine().Split(' ');
                string name = tokens[0];
                float x = float.Parse(tokens[1]);
                float y = float.Parse(tokens[2]);

                map.Add(new EnemyInfo(name, new Vector2(x, y)));
            }
            
            return map;
        }
    }
}

class MapInfo
{
    readonly int m_Rows;
    readonly int m_Cols;
    readonly int[,] m_Cells;
    readonly IList<EnemyInfo> m_Enemies;
    
    public MapInfo(int rows, int cols)
    {
        this.m_Cells = new int[rows, cols];
        this.m_Rows = rows;
        this.m_Cols = cols;
        this.m_Enemies = new List<EnemyInfo>();
    }
    
    public int this[int i, int j]
    {
        get
        {
            CheckCoords(i, j);
            return m_Cells[i, j];
        }
        
        set
        {
            CheckCoords(i, j);
            m_Cells[i, j] = value;
        }
    }

    public void Add(EnemyInfo enemy)
    {
        m_Enemies.Add(enemy);
    }
    
    void CheckCoords(int i, int j)
    {
        if (i < 0 || i >= m_Rows || j < 0 || j >= m_Cols)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public IList<EnemyInfo> enemies
    {
        get { return m_Enemies; }
    }
    
    public int rows
    {
        get { return m_Rows; }
    }
    
    public int cols
    {
        get { return m_Cols; }
    }
}

class EnemyInfo
{
    public readonly string name;
    public readonly Vector2 position;

    public EnemyInfo(string name, Vector2 position)
    {
        this.name = name;
        this.position = position;
    }
}
