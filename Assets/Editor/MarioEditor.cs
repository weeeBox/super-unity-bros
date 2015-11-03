using UnityEngine;
using UnityEditor;

using System.Collections;

using LunarCore;

[CustomEditor(typeof(MarioController))]
class MarioEditor : BaseEditor<MarioController>
{
    protected override void OnSceneGUI()
    {
//        Rect rect = target.colliderRect;
//        rect.center = target.transform.position;
//        DebugEx.DrawRect(rect);
    }
}
