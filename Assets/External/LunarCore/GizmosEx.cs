using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunarCore
{
    public static class GizmosEx
    {
        private static Stack<State> s_states = new Stack<State>();

        #region States

        public static void PushState()
        {
            s_states.Push(new State(Gizmos.matrix, Gizmos.color));
        }

        public static void PopState()
        {
            var state = s_states.Pop();
            Gizmos.matrix = state.matrix;
            Gizmos.color = state.color;
        }

        #endregion

        #region Drawing

        public static void DrawRect(Rect rect)
        {
            DrawRect(rect.x, rect.y, rect.width, rect.height);
        }

        public static void DrawRect(float x, float y, float w, float h)
        {
            DrawRect(x, y, w, h, Gizmos.color);
        }

        public static void DrawRect(float x, float y, float w, float h, Color color)
        {
            Color oldColor = Gizmos.color;

            Gizmos.color = color;
            Gizmos.DrawLine(new Vector3(x, y), new Vector3(x + w, y));
            Gizmos.DrawLine(new Vector3(x + w, y), new Vector3(x + w, y + h));
            Gizmos.DrawLine(new Vector3(x + w, y + h), new Vector3(x, y + h));
            Gizmos.DrawLine(new Vector3(x, y + h), new Vector3(x, y));

            Gizmos.color = oldColor;
        }

        #endregion
    }

    struct State
    {
        public readonly Matrix4x4 matrix;
        public readonly Color color;

        public State(Matrix4x4 matrix, Color color)
        {
            this.matrix = matrix;
            this.color = color;
        }
    }
}
