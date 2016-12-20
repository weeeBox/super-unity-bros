using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEditor;

[CustomEditor(typeof(QuestionTile))]
public class QuestionTileEditor : TileEditor<QuestionTile>
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


    }
}
