using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using UnityEditor;

using PhotoshopFile;

public class PsdMapImporter
{
    const int kTileWidth = 16;
    const int kTileHeight = 16;

    public static void ImportMap(string mapFile, string tileFile)
    {
        var psdFile = new PsdFile(mapFile, Encoding.UTF8);

        var document = new PsdDocument(psdFile);
        var width = document.width;
        var height = document.height;

        if (width % kTileWidth != 0)
        {
            throw new Exception("Invalid document width: " + width);
        }

        if (height % kTileHeight != 0)
        {
            throw new Exception("Invalid document height: " + width);
        }

        var tilesX = width / kTileWidth;
        var tilesY = height / kTileHeight;

        var mapLayer = document.FindLayer("Map");
        if (mapLayer == null)
        {
            throw new Exception("'Map' layer is missing");
        }


    }

    [MenuItem("File/Import map...")]
    private static void ImportMap()
    {
        string mapFile = "/Users/weeebox/dev/projects/unity/mario/Art/Levels/mario-1-1.psd";
        string tileFile = "";
        ImportMap(mapFile, tileFile);
    }
}