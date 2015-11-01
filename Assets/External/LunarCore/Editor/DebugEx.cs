using UnityEngine;

using System.Collections;

namespace LunarCore
{
    public static class DebugEx
    {
        public static void DrawRect(Rect rect)
        {
            DrawRect(rect.x, rect.y, rect.width, rect.height);
        }

        public static void DrawRect(float x, float y, float w, float h)
        {
            Debug.DrawLine(new Vector2(x, y), new Vector2(x + w, y));
            Debug.DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h));
            Debug.DrawLine(new Vector2(x + w, y + h), new Vector2(x, y + h));
            Debug.DrawLine(new Vector2(x, y + h), new Vector2(x, y));
        }
    }
}