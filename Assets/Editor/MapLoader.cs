using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Collections;

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
            
            return map;
        }
    }
}

class MapInfo
{
    readonly int m_Rows;
    readonly int m_Cols;
    readonly int[,] m_Cells;
    
    public MapInfo(int rows, int cols)
    {
        this.m_Cells = new int[rows, cols];
        this.m_Rows = rows;
        this.m_Cols = cols;
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
    
    void CheckCoords(int i, int j)
    {
        if (i < 0 || i >= m_Rows || j < 0 || j >= m_Cols)
        {
            throw new ArgumentOutOfRangeException();
        }
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
