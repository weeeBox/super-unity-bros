using UnityEngine;
using UnityEditor;

using System.Collections;

using LunarCore;

[CustomEditor(typeof(PlayerController))]
class MarioEditor : BaseEditor<PlayerController>
{
    protected override void OnSceneGUI()
    {
//        Rect rect = target.colliderRect;
//        rect.center = target.transform.position;
//        DebugEx.DrawRect(rect);
    }
}
