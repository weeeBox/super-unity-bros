using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using UnityEditor;

[CustomEditor(typeof(HittableTile))]
public class HittableTileEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        Rect totalPosition = EditorGUILayout.GetControlRect (false, 32, new GUILayoutOption[0]);
        totalPosition = EditorGUI.PrefixLabel (totalPosition, new GUIContent ("Preview"));
        Rect position = new Rect (totalPosition.xMin, totalPosition.yMin, 32, 32);
        Rect position2 = new Rect (totalPosition.xMin - 1, totalPosition.yMin - 1, 34, 34);
        if (Event.current.type == EventType.Repaint) {
            EditorStyles.textField.Draw (position2, false, false, false, false);
        }
        Sprite sprite = this.tile.sprite;
        if (sprite != null) {
            Texture2D image = SpriteUtility.RenderStaticPreview(sprite, this.tile.color, 32, 32, this.tile.transform);
            EditorGUI.DrawTextureTransparent (position, image, ScaleMode.StretchToFill);
        }
        EditorGUI.BeginChangeCheck ();
        EditorGUI.BeginChangeCheck ();
        Object obj = EditorGUILayout.ObjectField ("Sprite", this.tile.sprite, typeof(Sprite), false, new GUILayoutOption[] {
            GUILayout.Height (16)
        });
        if (EditorGUI.EndChangeCheck ()) {
            this.tile.sprite = (Sprite)obj;
        }
        if (EditorGUI.EndChangeCheck ()) {
            EditorUtility.SetDirty (this.tile);
        }
    }

    private HittableTile tile
    {
        get { return target as HittableTile; }
    }
}
