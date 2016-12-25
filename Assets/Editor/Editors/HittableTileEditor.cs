using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEditor;

public class HittableTileEditor<T> : TileEditor<T> where T : HittableTile
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        var type = EditorGUILayout.EnumPopup("Type", this.tile.type, new GUILayoutOption[]
            {
                GUILayout.Height(16)
            });
        if (EditorGUI.EndChangeCheck())
        {
            this.tile.type = (HittableTileType)type;
        }

        if (this.tile.type == HittableTileType.Coins)
        {
            EditorGUI.BeginChangeCheck();
            var cointCount = EditorGUILayout.IntField("Coins", this.tile.cointCount, new GUILayoutOption[]
                {
                    GUILayout.Height(16)
                });
            if (EditorGUI.EndChangeCheck())
            {
                this.tile.cointCount = cointCount;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(this.tile);
        }
    }
}
